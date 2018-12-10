
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CAFE.Web.Integration
{
    /// <summary>
    /// Client for interact with GfBio Service API
    /// </summary>
    public class GfBioServiceClient
    {
        private const string BasePath = @"https://terminologies.gfbio.org/api/terminologies/";
        private const string SearchPlaceholder = "query=%%SEARCH%%";
        private const string SearchPathWithPlaceholder = 
            BasePath + "search?format=json&match_type=regex";

        #region ctor

        public GfBioServiceClient()
        {

        }

        #endregion

        #region Methods


        public IEnumerable<GfBioTerminology> Search(string searchToken, string url = "")
        {
            var requestUrl = new StringBuilder(string.Concat(SearchPathWithPlaceholder, "&",
                SearchPlaceholder.Replace("%%SEARCH%%", searchToken)));

            if (!string.IsNullOrEmpty(url))
            {
                requestUrl = new StringBuilder(url);
                if (!requestUrl.ToString().EndsWith("&"))
                    requestUrl.Append("&");

                requestUrl.Append(SearchPlaceholder.Replace("%%SEARCH%%", searchToken));
            }
            HttpWebRequest request =
                WebRequest.CreateHttp(new Uri(requestUrl.ToString()));
            request.Method = "GET";

            GfBioResponse result = default(GfBioResponse);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<GfBioResponse>(responseText);
                }
            }
            catch (Exception ex)
            {
                //TODO: Log here
            }

            if (result != null)
            {
                return result.Results;
            }
            return Enumerable.Empty<GfBioTerminology>();
        }

        #endregion
    }


    [Serializable]
    public class GfBioResponse
    {
        public IEnumerable<GfBioTerminology> Results { get; set; }
    }

    [Serializable]
    public class GfBioTerminology
    {
        public string Label { get; set; }
        public string Uri { get; set; }
    }

}