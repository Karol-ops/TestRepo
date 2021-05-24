using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServiceAutomation.Helper.Authentication;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.PutEndPoint
{
    [TestClass]
    public class TestPutEndpoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add";
        private string securePutUrl = "http://localhost:8080/laptop-bag/webapi/secure/update";
        private RestResponse restResponse;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod]
        public void TestPutUsingXmlData()
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
                {"Accept", "application/xml"}
            };

            restResponse =  HttpClientHelper.PerformPostRequest(postUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200,restResponse.StatusCode);

            xmlData = "<Laptop>" +
                      "<BrandName>Alienware</BrandName>" +
                      "<Features>" +
                      "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                      "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                      "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                      "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                      "<Feature>1 TB of SSD</Feature>" +
                      "</Features>" +
                      "<Id>" + id + "</Id>" +
                      "<LaptopName>Alienware M17</LaptopName>" +
                      "</Laptop>";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);

                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
                restResponse = new RestResponse((int) httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                Assert.AreEqual(200, restResponse.StatusCode);

            }

            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            Laptop xmlDataLaptop = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsTrue(xmlDataLaptop.Features.Feature.Contains("1 TB of SSD"));
        }

        [TestMethod]
        public void TestPutUsingJsonData()
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
                {"Accept", "application/json"}
            };

            restResponse = HttpClientHelper.PerformPostRequest(postUrl, jsonData, jsonMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            jsonData = "{" +
                       "\"BrandName\": \"Alienware\"," +
                       "\"Features\": {" +
                       "\"Feature\": [" +
                       "\"8th Generation Intel® Core™ i5-8300H\"," +
                       "\"Windows 10 Home 64-bit English\"," +
                       "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                       "\"1 TB of SSD\"," +
                       "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                       "]" +
                       "}," +
                       "\"Id\": " + id + "," +
                       "\"LaptopName\": \"Alienware M17\"" +
                       "}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);

                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                Assert.AreEqual(200, restResponse.StatusCode);

            }

            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);

            JsonRootObject jsonObject =
                ResponseDataHelper.DeserializeJsonResponse<JsonRootObject>(restResponse.ResponseContent);
            Assert.IsTrue(jsonObject.Features.Feature.Contains("1 TB of SSD"));
        }

        [TestMethod]
        public void TestPutWithHelperClassJson()
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
                {"Accept", "application/json"}
            };

            restResponse = HttpClientHelper.PerformPostRequest(postUrl, jsonData, jsonMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            jsonData = "{" +
                       "\"BrandName\": \"Alienware\"," +
                       "\"Features\": {" +
                       "\"Feature\": [" +
                       "\"8th Generation Intel® Core™ i5-8300H\"," +
                       "\"Windows 10 Home 64-bit English\"," +
                       "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                       "\"1 TB of SSD\"," +
                       "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                       "]" +
                       "}," +
                       "\"Id\": " + id + "," +
                       "\"LaptopName\": \"Alienware M17\"" +
                       "}";


            restResponse = HttpClientHelper.PerformPutRequest(putUrl, jsonData,jsonMediaType,headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);

            JsonRootObject jsonObject =
                ResponseDataHelper.DeserializeJsonResponse<JsonRootObject>(restResponse.ResponseContent);
            Assert.IsTrue(jsonObject.Features.Feature.Contains("1 TB of SSD"));
        }

        [TestMethod]
        public void TestPutWithHelperClassXmlData()
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
                {"Accept", "application/xml"}
            };

            restResponse = HttpClientHelper.PerformPostRequest(postUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            xmlData = "<Laptop>" +
                      "<BrandName>Alienware</BrandName>" +
                      "<Features>" +
                      "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                      "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                      "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                      "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                      "<Feature>1 TB of SSD</Feature>" +
                      "</Features>" +
                      "<Id>" + id + "</Id>" +
                      "<LaptopName>Alienware M17</LaptopName>" +
                      "</Laptop>";

            restResponse = HttpClientHelper.PerformPutRequest(putUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            Laptop xmlDataLaptop = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsTrue(xmlDataLaptop.Features.Feature.Contains("1 TB of SSD"));
        }

        [TestMethod]
        public void TestSecurePutWithHelperClassXmlData()
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

            string authHeader = "Basic " + Base64StringConverter.GetBase64String("admin", "welcome");

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept", "application/xml"},
                { "Authorization", authHeader}
            };

            restResponse = HttpClientHelper.PerformPostRequest(securePostUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            xmlData = "<Laptop>" +
                      "<BrandName>Alienware</BrandName>" +
                      "<Features>" +
                      "<Feature>8th Generation Intel® Core™ i5 - 8300H</Feature>" +
                      "<Feature>Windows 10 Home 64 - bit English</Feature>" +
                      "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
                      "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                      "<Feature>1 TB of SSD</Feature>" +
                      "</Features>" +
                      "<Id>" + id + "</Id>" +
                      "<LaptopName>Alienware M17</LaptopName>" +
                      "</Laptop>";

            restResponse = HttpClientHelper.PerformPutRequest(securePutUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl + id, headers);
            Laptop xmlDataLaptop = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsTrue(xmlDataLaptop.Features.Feature.Contains("1 TB of SSD"));
        }
    }
}
