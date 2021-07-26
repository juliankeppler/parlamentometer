using System;
using System.Net;



namespace sweproject {

    /// <summary>Interface over <see cref="System.Net.WebClient"/> for unit testing.</summary>
    public interface IWebClient {

        /// <summary>Downloads the requested resource as a <see cref="System.String"/>. The resource to download is specified as a <see cref="System.String"/> containing the URI.</summary>
        /// <param name="address">A <see cref="System.String"/> containing a URI specifying the resource to download.</param>
        /// <returns>A <see cref="System.String"/> containing the requested resource.</returns>
        public string DownloadString(string address);

        /// <summary>Gets or sets a collection of header name/value pairs associated with the request.</summary>
        public WebHeaderCollection Headers { get; set; }
    }


    /// <summary>System web client.</summary>
    public class SystemWebClient : WebClient, IWebClient {

    }
}