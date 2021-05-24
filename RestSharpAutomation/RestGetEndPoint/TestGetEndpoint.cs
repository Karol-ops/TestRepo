using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Deserializers;
using RestSharpAutomation.HelperClass.Request;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace RestSharpAutomation.RestGetEndPoint
{
    [TestClass]
    public class TestGetEndpoint
    {
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/all";

        [TestMethod, TestCategory("RestGet")]
        public void TestGetUsingRestSharp()
        {
            IRestClient restClient = new RestClient(getUrl);
            IRestRequest restRequest = new RestRequest(Method.GET);

            IRestResponse restResponse = restClient.Get(restRequest);
            Assert.AreEqual(HttpStatusCode.OK,restResponse.StatusCode);

            Console.WriteLine(restResponse.Content);
        }

        [TestMethod, TestCategory("RestGet")]
        public void TestGetUsingRestSharp_Xml()
        {
            IRestClient restClient = new RestClient(getUrl);
            IRestRequest restRequest = new RestRequest(Method.GET);
            restRequest.AddHeader("Accept", "application/xml");

            IRestResponse restResponse = restClient.Get(restRequest);
            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);

            Console.WriteLine(restResponse.Content);
        }

        [TestMethod, TestCategory("RestGet")]
        public void TestGetInJsonDeserialize()
        {
            IRestClient restClient = new RestClient(getUrl);
            IRestRequest restRequest = new RestRequest(Method.GET);
            restRequest.AddHeader("Accept", "application/json");

            IRestResponse<List<JsonRootObject>> restResponse = restClient.Get<List<JsonRootObject>>(restRequest);
            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);

            Console.WriteLine(restResponse.Content);

            List<JsonRootObject> data = restResponse.Data;

            Assert.AreEqual("Alienware M17", data.Find(d => d.Id == 1).LaptopName);
            Assert.IsTrue(data.Find(d => d.Id == 1).Features.Feature.Contains("Windows 10 Home 64-bit English"));
        }

        [TestMethod, TestCategory("RestGet")]
        public void TestGetInXmlDeserialize()
        {
            IRestClient restClient = new RestClient(getUrl);
            IRestRequest restRequest = new RestRequest(Method.GET);
            restRequest.AddHeader("Accept", "application/xml");

            var dotNetXmlDeserializer = new DotNetXmlDeserializer();

            IRestResponse restResponse = restClient.Get(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);

            Console.WriteLine(restResponse.Content);

            LaptopDetailss data = dotNetXmlDeserializer.Deserialize<LaptopDetailss>(restResponse);

            Assert.AreEqual("Alienware M17", data.Laptop.Find(l => l.Id == "1").LaptopName);
            Assert.IsTrue(data.Laptop.Find(l => l.Id == "1").Features.Feature.Contains("Windows 10 Home 64-bit English"));
        }

        [TestMethod, TestCategory("RestGet")]
        public void TestGetWithExecute()
        {
            IRestClient restClient = new RestClient(getUrl);
            IRestRequest restRequest = new RestRequest()
            {
                Method = Method.GET,
                Resource = getUrl
            };

            restRequest.AddHeader("Accept", "application/json");
            IRestResponse restResponse = restClient.Execute<List<Laptop>>(restRequest);

            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);

            Console.WriteLine(restResponse.Content);
        }

        [TestMethod, TestCategory("RestGet")]
        public void TestGetWithXmlUsingHelperClass()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>() {{"Accept", "application/xml"}};

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getUrl, headers);

            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
            Console.WriteLine(restResponse.Content);

            IRestResponse<LaptopDetailss> restResponse1 = restClientHelper.PerformGetRequest<LaptopDetailss>(getUrl, headers);

            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Assert.IsNotNull(restResponse1.Data,"Content is null/empty");
        }

        [TestMethod, TestCategory("RestGet")]
        public void TestGetWithJsonUsingHelperClass()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>() { { "Accept", "application/json" } };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getUrl, headers);

            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
            Console.WriteLine(restResponse.Content);

            IRestResponse<List<Laptop>> restResponse1 = restClientHelper.PerformGetRequest<List<Laptop>>(getUrl, headers);

            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Assert.IsNotNull(restResponse1.Data, "Content is null/empty");
        }

    }
}
