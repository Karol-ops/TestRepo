using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model;

namespace WebServiceAutomation.Helper.Request
{
    public class HttpClientAsyncHelper
    {
        private HttpClient httpClient;
        private HttpRequestMessage httpRequestMessage;
        private RestResponse restResponse;

        private HttpClient AddHeaderAndCreateHttpClient(Dictionary<string,string> httpHeader)
        {
            HttpClient httpClient = new HttpClient();

            if (null != httpHeader)
            {
                foreach (string key in httpHeader.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, httpHeader[key]);
                }
            }


            return httpClient;
        }

        //private HttpRequestMessage CreateHttpRequestMessage(string requestUrl, HttpMethod httpMethod, HttpContent httpContent)
        //{
        //    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod,requestUrl);
        //    if (!(httpMethod == HttpMethod.Get))
        //        httpRequestMessage.Content = httpContent;

        //    return httpRequestMessage;
        //}

        //private RestResponse SendRequest(string requestUrl, HttpMethod httpMethod, HttpContent httpContent, Dictionary<string, string> httpHeader)
        //{
        //    httpClient = AddHeaderAndCreateHttpClient(httpHeader);
        //    httpRequestMessage = CreateHttpRequestMessage(requestUrl, httpMethod, httpContent);

        //    try
        //    {
        //        Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
        //        restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode , httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
        //    }
        //    catch (Exception err)
        //    {
        //        restResponse = new RestResponse(500,err.Message);
        //    }
        //    finally
        //    {
        //        httpRequestMessage?.Dispose();
        //        httpClient?.Dispose();
        //    }

        //    return restResponse;
        //}

        public async Task<RestResponse> PerformGetRequest(string requestUrl, Dictionary<string, string> httpHeader)
        {
            httpClient = AddHeaderAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUrl);
            int statusCode = (int) httpResponseMessage.StatusCode;
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();

            return new RestResponse(statusCode, responseData);
        }

        public async Task<RestResponse> PerformPostRequest(string requestUrl, HttpContent httpContent,
            Dictionary<string, string> httpHeader)
        {
            httpClient = AddHeaderAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUrl, httpContent);
            int statusCode = (int)httpResponseMessage.StatusCode;
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();

            return new RestResponse(statusCode, responseData);
        }

        //public async Task<RestResponse> PerformPostRequest(string requestUrl, string data, string mediaType,
        //    Dictionary<string, string> httpHeader)
        //{
        //    HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);

        //    httpClient = AddHeaderAndCreateHttpClient(httpHeader);
        //    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUrl, httpContent);
        //    int statusCode = (int)httpResponseMessage.StatusCode;
        //    var responseData = await httpResponseMessage.Content.ReadAsStringAsync();

        //    return new RestResponse(statusCode, responseData);
        //}

        public async Task<RestResponse> PerformPostRequest(string requestUrl, string data, string mediaType,
            Dictionary<string, string> httpHeader)
        {
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);

            return await PerformPostRequest(requestUrl, httpContent, httpHeader);
        }

        //public RestResponse PerformPostRequest(string requestUrl, string data, string mediaType,
        //    Dictionary<string, string> httpHeader)
        //{
        //    HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);
        //    return PerformPostRequest(requestUrl, httpContent, httpHeader);
        //}

        //public RestResponse PerformPutRequest(string requestUrl, HttpContent httpContent,
        //    Dictionary<string, string> httpHeader)
        //{
        //    return SendRequest(requestUrl, HttpMethod.Put, httpContent, httpHeader);
        //}

        public async Task<RestResponse> PerformPutRequest(string requestUrl, HttpContent httpContent,
            Dictionary<string, string> httpHeader)
        {
            httpClient = AddHeaderAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(requestUrl, httpContent);
            int statusCode = (int)httpResponseMessage.StatusCode;
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();

            return new RestResponse(statusCode, responseData);
        }

        public async Task<RestResponse> PerformPutRequest(string requestUrl, string data, string mediaType,
            Dictionary<string, string> httpHeader)
        {
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);

            return await PerformPutRequest(requestUrl, httpContent, httpHeader);
        }

        //public RestResponse PerformPutRequest(string requestUrl, string data, string mediaType,
        //    Dictionary<string, string> httpHeader)
        //{
        //    HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);
        //    return PerformPutRequest(requestUrl, httpContent, httpHeader);
        //}

        //public RestResponse PerformDeleteRequest(string requestUrl)
        //{
        //    return SendRequest(requestUrl, HttpMethod.Delete, null, null);
        //}

        public async Task<RestResponse> PerformDeleteRequest(string requestUrl, Dictionary<string,string> httpHeader)
        {
            httpClient = AddHeaderAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(requestUrl);
            int statusCode = (int)httpResponseMessage.StatusCode;
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();

            return new RestResponse(statusCode,responseData);
        }
    }
}
