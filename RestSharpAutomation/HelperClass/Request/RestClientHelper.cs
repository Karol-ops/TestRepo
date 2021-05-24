using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace RestSharpAutomation.HelperClass.Request
{
    public class RestClientHelper
    {
        private IRestClient GetRestClient()
        {
            return new RestClient();
        }

        private IRestRequest GetRestRequest(string url, Dictionary<string, string> headers, Method method, object body, DataFormat dataFormat)
        {
            IRestRequest restRequest = new RestRequest()
            {
                Method = method,
                Resource = url
            };

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    restRequest.AddHeader(key, headers[key]);
                }
            }

            if (body != null)
            {
                restRequest.RequestFormat = dataFormat;

                switch (dataFormat)
                {
                    case DataFormat.Json:
                        restRequest.AddBody(body);
                        break;
                    case DataFormat.Xml:
                        restRequest.XmlSerializer = new DotNetXmlSerializer();
                        restRequest.AddParameter("xmlData",
                            body.GetType().Equals(typeof(string))
                                ? body
                                : restRequest.XmlSerializer.Serialize(body), ParameterType.RequestBody);
                        restRequest.AddBody(body);
                        break;
                }

                
            }

            return restRequest;
        }

        private IRestResponse SendRequest(IRestRequest restRequest)
        {
            IRestClient restClient = GetRestClient();
            IRestResponse restresponse =  restClient.Execute(restRequest);

            return restresponse;
        }

        private IRestResponse<T> SendRequest<T>(IRestRequest restRequest) where T : new()
        {
            IRestClient restClient = GetRestClient();
            IRestResponse<T> restresponse = restClient.Execute<T>(restRequest);

            if (restresponse.ContentType.Equals("application/xml"))
            {
                var deserializer = new DotNetXmlDeserializer();
                restresponse.Data = deserializer.Deserialize<T>(restresponse);
            }

            return restresponse;
        }

        public IRestResponse PerformGetRequest(string url, Dictionary<string, string> headers)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.GET, null, DataFormat.None);

            return SendRequest(restRequest);
        }

        public IRestResponse<T> PerformGetRequest<T>(string url, Dictionary<string, string> headers) where T : new()
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.GET, null, DataFormat.None);

            return SendRequest<T>(restRequest);
        }

        public IRestResponse PerformPostRequest(string url, Dictionary<string, string> headers, object body, DataFormat dataFormat)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.POST, body, dataFormat);

            return SendRequest(restRequest);
        }

        public IRestResponse<T> PerformPostRequest<T>(string url, Dictionary<string, string> headers, object body, DataFormat dataFormat) where T : new()
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.POST, body, dataFormat);

            return SendRequest<T>(restRequest);
        }

        public IRestResponse PerformPutRequest(string url, Dictionary<string, string> headers, object body, DataFormat dataFormat)
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.PUT, body, dataFormat);

            return SendRequest(restRequest);
        }

        public IRestResponse<T> PerformPutRequest<T>(string url, Dictionary<string, string> headers, object body, DataFormat dataFormat) where T : new()
        {
            IRestRequest restRequest = GetRestRequest(url, headers, Method.PUT, body, dataFormat);

            return SendRequest<T>(restRequest);
        }

        public IRestResponse PerformDeleteRequest(string url)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>() {{"Accept", "*/*"}};

            IRestRequest restRequest = GetRestRequest(url, headers, Method.DELETE, null, DataFormat.None);

            return SendRequest(restRequest);
        }
    }
}
