using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WebServiceAutomation.Helper.Authentication;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.GetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/all";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/all";
        private string delayedGetUrl = "http://localhost:8080/laptop-bag/webapi/delay/all";

        [TestMethod]
        public void TestGetAllEndPoint()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.GetAsync(getUrl);
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointWithUrl()
        {
            HttpClient httpClient = new HttpClient();
            Uri getUri = new Uri(getUrl);
            var httpResponse = httpClient.GetAsync(getUri);
            var httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage);
            Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
            Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

            var responseContent = httpResponseMessage.Content;
            var responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;

            Console.WriteLine($"Content {data}");

            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointInJsonFormat()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            Uri getUri = new Uri(getUrl);
            var httpResponse = httpClient.GetAsync(getUri);
            var httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage);
            Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
            Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

            var responseContent = httpResponseMessage.Content;
            var responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;

            Console.WriteLine($"Content {data}");

            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointInXmlFormat()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");
            Uri getUri = new Uri(getUrl);
            var httpResponse = httpClient.GetAsync(getUri);
            var httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage);
            Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
            Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

            var responseContent = httpResponseMessage.Content;
            var responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;

            Console.WriteLine($"Content {data}");

            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointInXmlFormatUsingAcceptHeader()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            Uri getUri = new Uri(getUrl);
            var httpResponse = httpClient.GetAsync(getUri);
            var httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage);
            Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
            Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

            var responseContent = httpResponseMessage.Content;
            var responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;

            Console.WriteLine($"Content {data}");

            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointWithInvalidUrl()
        {
            HttpClient httpClient = new HttpClient();
            Uri getUri = new Uri(getUrl + "fakeUrl");
            var httpResponse = httpClient.GetAsync(getUri);
            var httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage);
            Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
            Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

            var responseContent = httpResponseMessage.Content;
            var responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;

            Console.WriteLine($"Content {data}");

            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetEndPointUsingSendAsync()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri(getUrl);
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            HttpClient httpClient = new HttpClient();
            var httpResponseMessage =  httpClient.SendAsync(httpRequestMessage).Result;

            Console.WriteLine(httpResponseMessage);
            Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
            Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

            var responseContent = httpResponseMessage.Content;
            var responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;

            Console.WriteLine($"Content {data}");

            httpClient.Dispose();
        }

        [TestMethod]
        public void TestUsingStatement()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    var httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        //Console.WriteLine(httpResponseMessage);
                        //Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
                        //Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

                        var responseContent = httpResponseMessage.Content;
                        var responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;

                        //Console.WriteLine($"Content {data}");

                        RestResponse restResponse = new RestResponse((int)httpResponseMessage.StatusCode, data);
                        Console.WriteLine(restResponse);
                    }
                }
            }
        }

        [TestMethod]
        public void TestDeserializationOfJsonResponse()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        //Console.WriteLine(httpResponseMessage);
                        //Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
                        //Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

                        var responseContent = httpResponseMessage.Content;
                        var responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;

                        //Console.WriteLine($"Content {data}");

                        RestResponse restResponse = new RestResponse((int)httpResponseMessage.StatusCode, data);
                        //Console.WriteLine(restResponse);
                        List<JsonRootObject> jsonRootObject = JsonConvert.DeserializeObject<List<JsonRootObject>>(restResponse.ResponseContent);
                        Console.WriteLine(jsonRootObject[0].ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void TestDeserializationOfXmlResponse()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    var httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        //Console.WriteLine(httpResponseMessage);
                        //Console.WriteLine($"Status Code {httpResponseMessage.StatusCode}");
                        //Console.WriteLine($"Status Code {(int)httpResponseMessage.StatusCode}");

                        var responseContent = httpResponseMessage.Content;
                        var responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;

                        //Console.WriteLine($"Content {data}");

                        RestResponse restResponse = new RestResponse((int)httpResponseMessage.StatusCode, data);
                        //Console.WriteLine(restResponse);
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopDetailss));
                        TextReader textReader = new StringReader(restResponse.ResponseContent);
                        LaptopDetailss xmlData =  (LaptopDetailss) xmlSerializer.Deserialize(textReader);
                        Console.WriteLine(xmlData.ToString());

                        Assert.AreEqual(200, (int)httpResponseMessage.StatusCode);
                        Assert.IsNotNull(restResponse.ResponseContent);
                        Assert.IsTrue(xmlData.Laptop[0].Features.Feature.Contains("Windows 10 Home 64-bit English"), "Item not found");
                    }
                }
            }
        }

        [TestMethod]
        public void GetUsingHelperMethod()
        {
            Dictionary<string,string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");

            RestResponse restResponse = HttpClientHelper.PerformGetRequest(getUrl, httpHeader);

            List<JsonRootObject> jsonData = ResponseDataHelper.DeserializeJsonResponse<List<JsonRootObject>>(restResponse.ResponseContent);

            Console.WriteLine(jsonData.ToString());
        }

        [TestMethod]
        public void TestGetSecureEndpoint()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            //httpHeader.Add("Authorization", "Basic YWRtaW46d2VsY29tZQ==");
            string authHeader = "Basic " + Base64StringConverter.GetBase64String("admin", "welcome");
            httpHeader.Add("Authorization", authHeader);

            RestResponse restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl, httpHeader);

            Assert.AreEqual(200,restResponse.StatusCode);

            List<JsonRootObject> jsonData = ResponseDataHelper.DeserializeJsonResponse<List<JsonRootObject>>(restResponse.ResponseContent);

            Console.WriteLine(jsonData.ToString());
        }

        [TestMethod]
        public void TestGetEndPoint_Sync()
        {
            HttpClientHelper.PerformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/all", null);

            HttpClientHelper.PerformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/all", null);

            HttpClientHelper.PerformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/all", null);

            HttpClientHelper.PerformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/all", null);
        }

        [TestMethod]
        public void TestGetEndPoint_Async()
        {
            Task t1 = new Task(GetEndPoint());
            t1.Start();
            Task t2 = new Task(GetEndPoint());
            t2.Start();
            Task t3 = new Task(GetEndPoint());
            t3.Start();
            Task t4 = new Task(GetEndPoint());
            t4.Start();

            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();
        }

        private Action GetEndPoint()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");

            return new Action(() =>
            {
                RestResponse restResponse = HttpClientHelper.PerformGetRequest(delayedGetUrl, httpHeader);
                Assert.AreEqual(200, restResponse.StatusCode);
            });
        }

    }
}
