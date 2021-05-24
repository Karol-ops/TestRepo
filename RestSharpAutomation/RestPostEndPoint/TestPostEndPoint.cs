using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serializers;
using RestSharpAutomation.HelperClass.Request;
using WebServiceAutomation.Model.XmlModel;
using RestResponse = WebServiceAutomation.Model.RestResponse;

namespace RestSharpAutomation.RestPostEndPoint
{
    [TestClass]
    public class TestPostEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add";
        private RestResponse restResponse;
        private RestResponse restResponseForGet;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod, TestCategory("RestPost")]
        public void TestPostWithJsonData()
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

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = postUrl
            };

            restRequest.AddHeader("Content-Type", jsonMediaType);
            restRequest.AddHeader("Accept", xmlMediaType);
            restRequest.AddJsonBody(jsonData);

            IRestResponse restResponse = restClient.Post(restRequest);

            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content);
        }

        private Laptop GetLaptopObject()
        {
            Laptop  laptop = new Laptop();
            laptop.BrandName = "Sample brand name";
            laptop.LaptopName = "Sample laptop name";
            laptop.Id = random.Next(1000).ToString();

            Features features = new Features();
            List<string> featureList = new List<string>()
            {
                ("Sample feature")
            };

            features.Feature = featureList;

            laptop.Features = features;

            return laptop;
        }

        [TestMethod, TestCategory("RestPost")]
        public void TestPostWithModelObject()
        {
            IRestClient restClient = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = postUrl
            };

            request.AddHeader("Content-Type", jsonMediaType);
            request.AddHeader("Accept", xmlMediaType);
            //request.RequestFormat = DataFormat.Json;
            //request.AddBody(GetLaptopObject());
            request.AddJsonBody(GetLaptopObject());

            IRestResponse response = restClient.Post(request);

            Assert.AreEqual(200, (int) response.StatusCode);
            Console.WriteLine(response.Content);
        }

        [TestMethod, TestCategory("RestPost")]
        public void TestPostWithModelObject_HelperClass()
        {
            IRestClient restClient = new RestClient();
            IRestRequest request = new RestRequest();

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Content-Type", jsonMediaType},
                {"Accept", xmlMediaType}
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse response =  restClientHelper.PerformPostRequest<Laptop>(postUrl, headers, GetLaptopObject(), DataFormat.Json);

            Assert.AreEqual(200, (int)response.StatusCode);
            Console.WriteLine(response.Content);
        }

        [TestMethod, TestCategory("RestPost")]
        public void TestPostWithXmlData()
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

            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = postUrl
            };

            request.AddHeader("Content-Type", xmlMediaType);
            request.AddHeader("Accept", xmlMediaType);

            request.AddParameter("XmlBody", xmlData, ParameterType.RequestBody);

            IRestResponse<Laptop> response = client.Post<Laptop>(request);

            Assert.AreEqual(200, (int)response.StatusCode);
            Console.WriteLine(response.Content);
        }

        [TestMethod, TestCategory("RestPost")]
        public void TestPostWithXmlData_ComplexPayload()
        {
            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = postUrl
            };

            request.AddHeader("Content-Type", xmlMediaType);
            request.AddHeader("Accept", xmlMediaType);
            request.RequestFormat = DataFormat.Xml;
            request.XmlSerializer = new DotNetXmlSerializer();

            request.AddParameter("XmlBody", request.XmlSerializer.Serialize(GetLaptopObject()), ParameterType.RequestBody);
            //request.AddXmlBody(GetLaptopObject()); // <- works without AddParameter

            IRestResponse<Laptop> response = client.Post<Laptop>(request);

            Assert.AreEqual(200, (int)response.StatusCode);
            Console.WriteLine(response.Content);
        }

        [TestMethod, TestCategory("RestPost")]
        public void TestPostWithXml_HelperClass()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Content-Type", xmlMediaType},
                {"Accept", xmlMediaType}
            };

            RestClientHelper helper = new RestClientHelper();
            IRestResponse<Laptop> response = helper.PerformPostRequest<Laptop>(postUrl, headers, GetLaptopObject(), DataFormat.Xml);

            Assert.AreEqual(200, (int)response.StatusCode);
            Console.WriteLine(response.Content);

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

            response = helper.PerformPostRequest<Laptop>(postUrl, headers, xmlData, DataFormat.Xml);

            Assert.AreEqual(200, (int)response.StatusCode);
            Console.WriteLine(response.Content);
        }


    }
}
