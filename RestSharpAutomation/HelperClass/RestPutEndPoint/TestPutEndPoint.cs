using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Deserializers;
using RestSharpAutomation.HelperClass.Request;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;
using RestResponse = WebServiceAutomation.Model.RestResponse;

namespace RestSharpAutomation.HelperClass.RestPutEndPoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add";
        private string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        private RestResponse restResponse;
        private RestResponse restResponseForGet;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod, TestCategory("RestPut")]
        public void TestPutWithJsonData()
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

            jsonData = "{" +
                       "\"BrandName\": \"Alienware\"," +
                       "\"Features\": {" +
                       "\"Feature\": [" +
                       "\"8th Generation Intel® Core™ i5-8300H\"," +
                       "\"Windows 10 Home 64-bit English\"," +
                       "\"New Feature\"," +
                       "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                       "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                       "]" +
                       "}," +
                       "\"Id\": " + id + "," +
                       "\"LaptopName\": \"Alienware M17\"" +
                       "}";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = putUrl
            };

            restRequest.AddHeader("Content-Type", jsonMediaType);
            restRequest.AddHeader("Accept", jsonMediaType);
            //restRequest.RequestFormat = DataFormat.Json;
            //restRequest.AddBody(jsonData);
            restRequest.AddJsonBody(jsonData);

            IRestResponse<JsonRootObject> putResponse = restClient.Put<JsonRootObject>(restRequest);

            Assert.AreEqual(200, (int)putResponse.StatusCode);
            Assert.IsNotNull(putResponse.Content);
            Assert.IsTrue(putResponse.Data.Features.Feature.Contains("New Feature"));

            IRestResponse<JsonRootObject> getResponse =  helper.PerformGetRequest<JsonRootObject>(getUrl + id, headers);

            Assert.AreEqual(200, (int)getResponse.StatusCode);
            Assert.IsNotNull(getResponse.Content);
            Assert.IsTrue(getResponse.Data.Features.Feature.Contains("New Feature"));

        }

        [TestMethod, TestCategory("RestPut")]
        public void TestPutWithXmlData()
        {
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                             "<BrandName>Alienware</BrandName>" +
                             "<Features>" +
                             "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                             "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                             "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                             "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                             "</Features>" +
                             "<Id>" + id + "</Id>" +
                             "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Content-Type", xmlMediaType},
                {"Accept", xmlMediaType}
            };

            RestClientHelper helper = new RestClientHelper();
            IRestResponse response = helper.PerformPostRequest(postUrl, headers, xmlData, DataFormat.Xml);

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Content);

            xmlData = "<Laptop>" +
                             "<BrandName>Alienware</BrandName>" +
                             "<Features>" +
                             "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                             "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                             "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                             "<Feature>New Feature</Feature>" +
                             "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                             "</Features>" +
                             "<Id>" + id + "</Id>" +
                             "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = putUrl
            };

            restRequest.AddHeader("Content-Type", xmlMediaType);
            restRequest.AddHeader("Accept", xmlMediaType);
            restRequest.RequestFormat = DataFormat.Xml;
            restRequest.AddParameter("xmlData", xmlData, ParameterType.RequestBody);

            IRestResponse putResponse = restClient.Put(restRequest);

            var deserializer = new DotNetXmlDeserializer();
            var laptop =  deserializer.Deserialize<Laptop>(putResponse);

            Assert.AreEqual(200, (int)putResponse.StatusCode);
            Assert.IsNotNull(putResponse.Content);
            Assert.IsTrue(laptop.Features.Feature.Contains("New Feature"));

            IRestResponse<Laptop> getResponse = helper.PerformGetRequest<Laptop>(getUrl + id, headers);

            Assert.AreEqual(200, (int)getResponse.StatusCode);
            Assert.IsNotNull(getResponse.Content);
            Assert.IsTrue(getResponse.Data.Features.Feature.Contains("New Feature"));
        }

        [TestMethod, TestCategory("RestPut")]
        public void TestPutWithXmlDataUsingHelper()
        {
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                             "<BrandName>Alienware</BrandName>" +
                             "<Features>" +
                             "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                             "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                             "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                             "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                             "</Features>" +
                             "<Id>" + id + "</Id>" +
                             "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Content-Type", xmlMediaType},
                {"Accept", xmlMediaType}
            };

            RestClientHelper helper = new RestClientHelper();
            IRestResponse response = helper.PerformPostRequest(postUrl, headers, xmlData, DataFormat.Xml);

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Content);

            xmlData = "<Laptop>" +
                      "<BrandName>Alienware</BrandName>" +
                      "<Features>" +
                      "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                      "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                      "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                      "<Feature>New Feature</Feature>" +
                      "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                      "</Features>" +
                      "<Id>" + id + "</Id>" +
                      "<LaptopName>Alienware M17</LaptopName>" +
                      "</Laptop>";

            IRestResponse<Laptop> putResponse =  helper.PerformPutRequest<Laptop>(putUrl, headers, xmlData, DataFormat.Xml);

            Assert.AreEqual(200, (int)putResponse.StatusCode);
            Assert.IsNotNull(putResponse.Content);
            Assert.IsTrue(putResponse.Data.Features.Feature.Contains("New Feature"));

            IRestResponse<Laptop> getResponse = helper.PerformGetRequest<Laptop>(getUrl + id, headers);

            Assert.AreEqual(200, (int)getResponse.StatusCode);
            Assert.IsNotNull(getResponse.Content);
            Assert.IsTrue(getResponse.Data.Features.Feature.Contains("New Feature"));
        }

        [TestMethod, TestCategory("RestPut")]
        public void TestPutWithJsonDataUsingHelper()
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

            jsonData = "{" +
                       "\"BrandName\": \"Alienware\"," +
                       "\"Features\": {" +
                       "\"Feature\": [" +
                       "\"8th Generation Intel® Core™ i5-8300H\"," +
                       "\"Windows 10 Home 64-bit English\"," +
                       "\"New Feature\"," +
                       "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                       "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                       "]" +
                       "}," +
                       "\"Id\": " + id + "," +
                       "\"LaptopName\": \"Alienware M17\"" +
                       "}";

            IRestResponse<JsonRootObject> putResponse =  helper.PerformPutRequest<JsonRootObject>(putUrl, headers, jsonData, DataFormat.Json);

            Assert.AreEqual(200, (int)putResponse.StatusCode);
            Assert.IsNotNull(putResponse.Content);
            Assert.IsTrue(putResponse.Data.Features.Feature.Contains("New Feature"));

            IRestResponse<JsonRootObject> getResponse = helper.PerformGetRequest<JsonRootObject>(getUrl + id, headers);

            Assert.AreEqual(200, (int)getResponse.StatusCode);
            Assert.IsNotNull(getResponse.Content);
            Assert.IsTrue(getResponse.Data.Features.Feature.Contains("New Feature"));

        }

    }
}
