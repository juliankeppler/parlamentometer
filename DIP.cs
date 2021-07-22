using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System;
using Newtonsoft.Json;


public class DIP {
    private static string APIKey = "SbGXhWA.3cpnNdb8rkht7iWpvSgTP8XIG88LoCrGd4";
    private static string URLBase = "https://search.dip.bundestag.de/search-api/v1/advanced/search";
    private WebClient wc = new WebClient();

    private string ToQueryString(NameValueCollection nvc) {
        var array = (
            from key in nvc.AllKeys
            from value in nvc.GetValues(key)
                select string.Format(
                    "{0}={1}",
                    HttpUtility.UrlEncode(key),
                    HttpUtility.UrlEncode(value))
            ).ToArray();
        return "?" + string.Join("&", array);
    }

    private dynamic Request(NameValueCollection args) {
        if (args["term"] == "") {
            throw new ArgumentException("No search term provided!");
        }

        string requestURL = URLBase+ToQueryString(args);

        string json = wc.DownloadString(requestURL);
        dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

        return obj;
    }
    public DIP() {
        wc.Headers.Add("Authorization", "ApiKey "+DIP.APIKey);
        wc.Headers.Add("Origin", "https://dip.bundestag.de");
        wc.Headers.Add("Referer", "https://dip.bundestag.de/");
    }

    public int GetResults(string term) {
        NameValueCollection args = new NameValueCollection();
        args.Add("term", term);
        args.Add("sort", "datum_auf");
        args.Add("rows", "1");
        dynamic resp;
        try {
            resp = Request(args);
        } catch (ArgumentException) {
            throw;
        }
        return resp.numFound;
    }
}