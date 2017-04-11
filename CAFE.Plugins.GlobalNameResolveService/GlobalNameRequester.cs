
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace CAFE.Plugins.GlobalNameResolveService
{
    public static class GlobalNameRequester
    {
        public static ResponseData GetNames(RequestData requestData)
        {
            ResponseData data = null;

            HttpWebRequest request = WebRequest.CreateHttp(new Uri("http://resolver.globalnames.org/name_resolvers.json"));
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var serializedData = JsonConvert.SerializeObject(requestData);
                streamWriter.Write(serializedData);
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    data = JsonConvert.DeserializeObject<ResponseData>(responseText);
                }
            }
            catch (Exception ex)
            {
                //TODO: Log here
                throw;
            }
            return data;
        }
    }
}
