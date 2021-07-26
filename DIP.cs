using System;
using System.Net;
using System.Web;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;


namespace sweproject {
    /// <summary>
    /// GroupMode provides different options for tallying up the results.
    /// </summary>
    public enum GroupMode{
            /// <summary>
            /// Results are grouped by Year.
            /// </summary>
            Year,
            /// <summary>
            /// Results are grouped by Month.
            /// </summary>
            Month
    }


    /// <summary>
    /// This class provides methods for gathering data from the DIP-API.
    /// </summary>
    public class DIP {
    
        private const string APIKey = "SbGXhWA.3cpnNdb8rkht7iWpvSgTP8XIG88LoCrGd4"; // The APIKey used to perform requests. This is the public API-Key which rotates once a year.
        private const string URLBase = "https://search.dip.bundestag.de/search-api/v1/advanced/search"; // The base URL used for all requests to the DIP-API
        private WebClient wc = new WebClient(); // The WebClient used to perform requests.
        private Dictionary<string, int> numFoundCache = new Dictionary<string, int>(){}; // cached numFounds for searches

        private static Dictionary<int, string> electoralPeriods = new Dictionary<int, string>(){
            {01, "1949-09"},
            {02, "1953-10"},
            {03, "1957-10"},
            {04, "1961-10"},
            {05, "1965-10"},
            {06, "1969-10"},
            {07, "1972-12"},
            {08, "1976-12"},
            {09, "1980-11"},
            {10, "1983-03"},
            {11, "1987-02"},
            {12, "1990-12"},
            {13, "1994-11"},
            {14, "1998-10"},
            {15, "2002-10"},
            {16, "2005-10"},
            {17, "2009-10"},
            {18, "2013-10"},
            {19, "2017-10"},
            {20, "2021-10"}
        }; // Dictionary of when which electoral period started

        /// <summary>Initializes a new instance of <see cref="DIP"/>.</summary>
        public DIP() {
            // Set authorization headers to access the advanced DIP API
            wc.Headers.Add("Authorization", "ApiKey "+DIP.APIKey);
            wc.Headers.Add("Origin", "https://dip.bundestag.de");
            wc.Headers.Add("Referer", "https://dip.bundestag.de/");
        }

        /// <summary>
        /// Converts a given NameValueCollection with query arguments into a query string.
        /// </summary>
        /// <param name="args">The <see cref="System.Collections.Specialized.NameValueCollection"/> holding the query arguments as key, value pairs.</param>
        /// <returns>A query <see cref="System.String"/>.</returns>
        private string ToQueryString(NameValueCollection args) {
            var array = (
                from key in args.AllKeys
                from value in args.GetValues(key)
                    select string.Format(
                        "{0}={1}",
                        HttpUtility.UrlEncode(key),
                        HttpUtility.UrlEncode(value))
                ).ToArray();
            return "?" + string.Join("&", array);
        }

        /// <summary>
        /// Performs an http request to the DIP API.
        /// </summary>
        /// <param name="args">A <see cref="System.Collections.Specialized.NameValueCollection"/> holding the query arguments for the request.</param>
        /// <returns>A <see cref="System.Dynamic"/> object representing the response of the API.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="args"/> holds invalid parameters.</exception>
        private dynamic Request(NameValueCollection args) {

            // Filter for invalid arguments
            if (args["term"] == "") {
                throw new ArgumentException("No search term provided!");
            }

            // Build request URL from args
            string requestURL = URLBase+ToQueryString(args);

            // Do request
            string json = wc.DownloadString(requestURL);

            // Convert json into dynamic object
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

            return obj;
        }

        /// <summary>
        /// Helper method that sets the standard parameters of a query string.
        /// </summary>
        /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
        /// <param name="electionPeriods">A <see cref="T:int[]"/> containing the selected election periods.</param>
        /// <returns>A <see cref="System.Collections.Specialized.NameValueCollection"/> holding the query args.</returns>
        private NameValueCollection SetDefaultParams(string term, int[] electionPeriods) {
            NameValueCollection args = new NameValueCollection();
            args.Add("term", term);
            args.Add("sort", "datum_auf");
            args.Add("f.aktivitaetsart_p", "05Reden, Wortmeldungen im Plenum");
            foreach (int i in electionPeriods) {
                args.Add("f.wahlperiode", i.ToString());
            }
            return args;
        }

        /// <summary>
        /// Retrieves the number of results found for a given search term.
        /// </summary>
        /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
        /// <param name="electionPeriods">A <see cref="T:int[]"/> containing the selected election periods.</param>
        /// <returns>An <see cref="System.Int32"/> representing the number of results found.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>
        public int GetResults(string term, int[] electionPeriods) {

            // Create request query
            NameValueCollection args = SetDefaultParams(term, electionPeriods);
            args.Add("rows", "0"); // We don't need to request any document data to get the number of results
            

            // Do the request
            dynamic resp;
            try {
                resp = Request(args);
            } catch (ArgumentException) {
                throw;
            }

            // Add number found to cache and return
            int n = resp.numFound;
            numFoundCache.Add(term+electionPeriods.ToString(), n);
            return n;
        }

        /// <summary>
        /// Retrieves the number of results found for a given search term.
        /// </summary>
        /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
        /// <returns>An <see cref="System.Int32"/> representing the number of results found.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>
        public int GetResults(string term) {
            return GetResults(term, new int[]{});
        }

        /// <summary>
        /// Gets a List containing the dates of each mention of the search term.
        /// </summary>
        /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
        /// <param name="electionPeriods">A <see cref="T:int[]"/> containing the selected election periods.</param>
        /// <returns>A <see cref="System.Collections.Generic.List{String}"/> of dates for each time the term was mentioned. Dates may appear multiple times when the term was mention more than once on a given day.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>
        /// <exception cref="System.InvalidOperationException">There are too many results for <paramref name="term"/> during <paramref name="electionPeriods"/>.</exception>
        private List<string> GetMentions(string term, int[] electionPeriods) {
            NameValueCollection args = SetDefaultParams(term, electionPeriods);

            int n = 0;
            if (numFoundCache.ContainsKey(term+electionPeriods.ToString())) {
                n = numFoundCache[term+electionPeriods.ToString()];
            } else {
                n = GetResults(term, electionPeriods);
            }
            
            int batchsize = 500;
            args.Add("rows", batchsize.ToString());
            int requests = (int)Math.Ceiling((decimal)n/batchsize);

            if (requests>20) {
                throw new InvalidOperationException("Too many results for given search term.");
            }

            List<string> res = new List<string>{};

            Console.WriteLine("Fetching data...");
            for (int i = 0; i < requests; i++)
            {
                args.Set("start", (i*batchsize).ToString());
                //Console.WriteLine($"n={n}, batchsize={batchsize}, i={i}, start={i*batchsize}, progress={String.Format("{0:P0}", (double)res.Count/n)}");
                Console.WriteLine($"Progress: {String.Format("{0:P0}", (double)res.Count/n)}");

                if (i%4==0 && i != 0) {
                    Thread.Sleep(1000);
                }

                dynamic resp;
                try {
                    resp = Request(args);
                } catch (ArgumentException) {
                    throw;
                }
                
                foreach (dynamic document in resp.documents) {
                    res.Add(document.datum.ToString());
                }
            }
            Console.WriteLine($"Progress: 100%");
            return res;
        }


        private SortedDictionary<string, int> FillZeroes(SortedDictionary<string, int> dict, GroupMode mode, int[] electionPeriods) {

            string first;
            string last;

            if (electionPeriods.Length != 0) {
                first = electoralPeriods[electionPeriods.Min()];
                last = electoralPeriods[electionPeriods.Max()+1];
            } else {
                first = electoralPeriods[8]; // There are only few speeches recorded before the 8th electoral term this that period is irrelevant and should not be filled with zeroes
                last = electoralPeriods[20];
            }

            switch (mode) {
            case GroupMode.Year:
                first = first.Substring(0,4);
                last = last.Substring(0,4);
                
                int x = Convert.ToInt32(first);
                int y = Convert.ToInt32(last);

                for (int i = x+1; i < y; i++) {
                    if (!dict.ContainsKey(i.ToString())) {
                        dict[i.ToString()] = 0;
                    }
                }
                break;
            case GroupMode.Month:

                int firsty = Convert.ToInt32(first.Substring(0,4));
                int firstm = Convert.ToInt32(first.Substring(5,2));
                int lasty = Convert.ToInt32(last.Substring(0,4));
                int lastm = Convert.ToInt32(last.Substring(5,2));
                for (int i = firstm+1; i<=12; i++) {

                    string key = firsty + "-" + i.ToString("00");
                    if (!dict.ContainsKey(key)) {
                        dict[key] = 0;
                    }
                }
                for (int i = firsty+1; i<lasty; i++) {
                    for (int j = 1; j <= 12; j++) {
                        string key = i + "-" + j.ToString("00");
                        if (!dict.ContainsKey(key)) {
                            dict[key] = 0;
                        }
                    }
                }
                for (int i = 1; i<lastm; i++) {
                    string key = lasty + "-" + i.ToString("00");
                    if (!dict.ContainsKey(key)) {
                        dict[key] = 0;
                    }
                }
                break;
            }

            return dict;
        }

        /// <summary>
        /// Groups all mentions of a search term by time.
        /// </summary>
        /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
        /// <param name="mode">A <see cref="GroupMode"/> determining whether the results should be grouped by Month or Year.</param>
        /// <param name="electionPeriods">An <see cref="T:int[]"/> containing the selected election periods.</param>
        /// <returns>A <see cref="System.Collections.Generic.SortedDictionary{String, Int32}"/> containing the amount of mentions of the <paramref name="term"/> grouped by time.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>

        /// <exception cref="System.InvalidOperationException">There are too many results for <paramref name="term"/> during <paramref name="electionPeriods"/>.</exception>
        public SortedDictionary<string, int> GetRelevance(string term, GroupMode mode, int[] electionPeriods) {

            
            List<string> mentions = GetMentions(term, electionPeriods);
            SortedDictionary<string, int> res = new SortedDictionary<string, int>();

            if (mentions.Count == 0) {
                throw new IndexOutOfRangeException("no results found");
            }

            foreach (string mention in mentions) {

                // trim strings to identify a month or year
                string m = mention;
                if (mode == GroupMode.Month) {
                    m = mention.Remove(mention.Length-3, 3);
                } else {
                    m = mention.Remove(mention.Length-6, 6);
                }

                // put mentions into buckets
                if (res.ContainsKey(m)) {
                    res[m]++;
                } else {
                    res[m] = 1;
                }
            }

            res = FillZeroes(res, mode, electionPeriods);

            return res;
        }

        /// <summary>
        /// Groups all mentions of a search term by time.
        /// </summary>
        /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
        /// <param name="mode">A <see cref="GroupMode"/> determining whether the results should be grouped by Month or Year.</param>
        /// <returns>A <see cref="System.Collections.Generic.Dictionary{String, Int32}"/> containing the amount of mentions of the <paramref name="term"/> grouped by time.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>

        /// <exception cref="System.InvalidOperationException">There are too many results for <paramref name="term"/>.</exception>

        public SortedDictionary<string, int> GetRelevance(string term, GroupMode mode) {
            return GetRelevance(term, mode, new int[]{});
        }

    }
}