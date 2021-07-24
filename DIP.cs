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
    
    public int GetResults(string term) {
        return GetResults(term, new int[]{});
    }

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
    
    public Dictionary<string, int> GetRelevance(string term, GroupMode mode, int[] electionPeriod) {
        
        List<string> mentions = GetMentions(term, electionPeriod);
        Dictionary<string, int> res = new Dictionary<string, int>();

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
        return res;
    }

    public Dictionary<string, int> GetRelevance(string term, GroupMode mode) {
        return GetRelevance(term, mode, new int[]{});
    }

}