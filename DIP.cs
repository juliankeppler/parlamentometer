using System;
using System.Net;
using System.Web;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;

public enum GroupMode{Year,Month}

public class DIP {
    /// <summary>
    /// Used to interact with the DIP API.
    /// </summary>
   
    private const string APIKey = "SbGXhWA.3cpnNdb8rkht7iWpvSgTP8XIG88LoCrGd4"; //The public API-Key for the DIP-API
    private const string URLBase = "https://search.dip.bundestag.de/search-api/v1/advanced/search"; //The base URL for all requests to the DIP-API
    private WebClient wc = new WebClient(); // The client used to perform all http requests to the DIP APi
    private Dictionary<string, int> numFoundCache = new Dictionary<string, int>{};
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
    /// <param name="electionPeriod">A <see cref="T:int[]"/> containing the selected election periods.</param>
    /// <returns>A <see cref="System.Collections.Specialized.NameValueCollection"/> holding the query args.</returns>
    private NameValueCollection SetDefaultParams(string term, int[] electionPeriod) {
        NameValueCollection args = new NameValueCollection();
        args.Add("term", term);
        args.Add("sort", "datum_auf");
        args.Add("f.aktivitaetsart_p", "05Reden, Wortmeldungen im Plenum");
        foreach (int i in electionPeriod) {
            args.Add("f.wahlperiode", i.ToString());
        }
        return args;
    }

    /// <summary>
    /// Retrieves the number of results found for a given search term.
    /// </summary>
    /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
    /// <param name="electionPeriod">A <see cref="T:int[]"/> containing the selected election periods.</param>
    /// <returns>An <see cref="System.Int32"/> representing the number of results found.</returns>
    /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>
    public int GetResults(string term, int[] electionPeriod) {

        // Create request query
        NameValueCollection args = SetDefaultParams(term, electionPeriod);
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
        numFoundCache.Add(term+electionPeriod.ToString(), n);
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
    /// <param name="electionPeriod">A <see cref="T:int[]"/> containing the selected election periods.</param>
    /// <returns>A <see cref="System.Collections.Generic.List{string}"/> of dates for each time the term was mentioned. Dates may appear multiple times when the term was mention more than once on a given day.</returns>
    /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>
    /// <exception cref="System.InvalidOperationException">There are too many results for <paramref name="term"/> during <paramref name="electionPeriod"/>.</exception>
    private List<string> GetMentions(string term, int[] electionPeriod) {
        NameValueCollection args = SetDefaultParams(term, electionPeriod);

        int n = 0;
        if (numFoundCache.ContainsKey(term+electionPeriod.ToString())) {
            n = numFoundCache[term+electionPeriod.ToString()];
        } else {
            n = GetResults(term, electionPeriod);
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
            Console.WriteLine($"n={n}, batchsize={batchsize}, i={i}, start={i*batchsize}, progress={String.Format("{0:P0}", (double)res.Count/n)}");

            if (i%4==0 && i != 0) {
                Console.WriteLine("Sleeping");
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
        return res;
    }

    private SortedDictionary<string, int> FillZeroes(SortedDictionary<string, int> dict, GroupMode mode) {

        string first = dict.Keys.First();
        string last = dict.Keys.Last();

        switch (mode) {
        case GroupMode.Year:
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
                dict[firsty + "-" + i.ToString("00")] = 0;
            }
            for (int i = firsty+1; i<lasty; i++) {
                for (int j = 1; j <= 12; j++) {
                    dict[i + "-" + j.ToString("00")] = 0;
                }
            }
            for (int i = 1; i<lastm; i++) {
                dict[lasty + "-" + i.ToString("00")] = 0;
            }
            break;
        }

        return dict;
    }

    /// <summary>
    /// Groups all mentions of a search term by time.
    /// </summary>
    /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
    /// <param name="term">A <see cref="GroupMode"/> determining whether the results should be grouped by Month or Year.</param>
    /// <param name="term">An <see cref="T:int[]"/> containing the selected election periods.</param>
    /// <returns>A <see cref="System.Collections.Generic.SortedDictionary{System.String, System.Int32}"/> containing the amount of mentions of the <paramref name="term"/> grouped by time.</returns>
    /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>
    /// <exception cref="System.InvalidOperationException">There are too many results for <paramref name="term"/> during <paramref name="electionPeriod"/>.</exception>
    public SortedDictionary<string, int> GetRelevance(string term, GroupMode mode, int[] electionPeriod) {
        
        List<string> mentions = GetMentions(term, electionPeriod);
        SortedDictionary<string, int> res = new SortedDictionary<string, int>();

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
        res = FillZeroes(res, mode);

        return res;
    }

    /// <summary>
    /// Groups all mentions of a search term by time.
    /// </summary>
    /// <param name="term">A <see cref="System.String"/> representing the search term.</param>
    /// <param name="term">A <see cref="GroupMode"/> determining whether the results should be grouped by Month or Year.</param>
    /// <returns>A <see cref="System.Collections.Generic.Dictionary{System.String, System.Int32}"/> containing the amount of mentions of the <paramref name="term"/> grouped by time.</returns>
    /// <exception cref="System.ArgumentException"><paramref name="term"/> is an invalid search term.</exception>
    /// <exception cref="System.InvalidOperationException">There are too many results for <paramref name="term"/> during <paramref name="electionPeriod"/>.</exception>
    public SortedDictionary<string, int> GetRelevance(string term, GroupMode mode) {
        return GetRelevance(term, mode, new int[]{});
    }

}