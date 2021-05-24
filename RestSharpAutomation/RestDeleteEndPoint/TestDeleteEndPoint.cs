using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using RestResponse = WebServiceAutomation.Model.RestResponse;

namespace RestSharpAutomation.RestDeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        private string deleteUrl = "http://localhost:8080/laptop-bag/webapi/api/delete/";
        private string secureDeleteUrl = "http://localhost:8080/laptop-bag/webapi/secure/delete/";
        private RestResponse restResponse;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod, TestCategory("ResrDelete")]
        public void TestDeleteRequest()
        {
            int id = random.Next(1000);
            string jsonData = "{" +
                              "\"BrandName\": \"Alienware\"," +
                              "\"Features\": {" +
                              "\"Feature\": [" +
                              "\"8th Generation Intel® Core™ i5-8300H\"," +
                              "\"Windows 10 Home 64-bit English\"," +
                              "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                              "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                              "]" +
                              "}," +
                              "\"Id\": " + id + "," +
                              "\"LaptopName\": \"Alienware M17\"" +
                              "}";

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Content-Type", jsonMediaType},
                {"Accept", jsonMediaType}
            };

            RestClientHelper helper = new RestClientHelper();
            IRestResponse response = helper.PerformPostRequest(postUrl, headers, jsonData, DataFormat.Json);

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Content);

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest() {Resource = deleteUrl + id};
            restRequest.AddHeader("Accept", "*/*");

            IRestResponse restResponse =  restClient.Delete(restRequest);

            Assert.AreEqual(200, (int)restResponse.StatusCode, "First delete failed");

            restResponse = restClient.Delete(restRequest);

            Assert.AreEqual(404, (int)restResponse.StatusCode, "Second delete failed");
        }

        [TestMethod, TestCategory("ResrDelete")]
        public void TestDeleteRequestUsingHelper()
        {
            int id = random.Next(1000);
            string jsonData = "{" +
                              "\"BrandName\": \"Alienware\"," +
                              "\"Features\": {" +
                              "\"Feature\": [" +
                              "\"8th Generation Intel® Core™ i5-8300H\"," +
                              "\"Windows 10 Home 64-bit English\"," +
                              "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                              "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                              "]" +
                              "}," +
                              "\"Id\": " + id + "," +
                              "\"LaptopName\": \"Alienware M17\"" +
                              "}";

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Content-Type", jsonMediaType},
                {"Accept", jsonMediaType}
            };

            RestClientHelper helper = new RestClientHelper();
            IRestResponse response = helper.PerformPostRequest(postUrl, headers, jsonData, DataFormat.Json);

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Content);

            response = helper.PerformDeleteRequest(deleteUrl + id);
            Assert.AreEqual(200, (int)response.StatusCode, "First delete failed");

            response = helper.PerformDeleteRequest(deleteUrl + id);
            Assert.AreEqual(404, (int)response.StatusCode, "Second delete failed");
        }
    }
}
