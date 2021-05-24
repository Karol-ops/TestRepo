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
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.DeleteEndPoint
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

        [TestMethod]
        public void TestDeleteData()
        {
            int id = random.Next(1000);

            AddRecord(id);

            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> httpResponseMessage = httpClient.DeleteAsync(deleteUrl + id);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                Assert.AreEqual(200, restResponse.StatusCode);
            }

            //restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            //Assert.AreEqual(404, restResponse.StatusCode);

            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> httpResponseMessage = httpClient.DeleteAsync(deleteUrl + id);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                Assert.AreEqual(404, restResponse.StatusCode);

            }
        }

        [TestMethod]
        public void TestDeleteUsingHelperClass()
        {
            int id = random.Next(1000);

            AddRecord(id);

            restResponse = HttpClientHelper.PerformDeleteRequest(deleteUrl + id);
            Assert.AreEqual(200, restResponse.StatusCode);

            //restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            //Assert.AreEqual(404, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformDeleteRequest(deleteUrl + id);
            Assert.AreEqual(404, restResponse.StatusCode);
        }

        [TestMethod]
        public void TestSecureDeleteUsingHelperClass()
        {
            int id = random.Next(1000);

            AddRecord(id);

            string authHeader = "Basic " + Base64StringConverter.GetBase64String("admin", "welcome");

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Authorization", authHeader}
            };

            restResponse = HttpClientHelper.PerformDeleteRequest(secureDeleteUrl + id, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            //restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            //Assert.AreEqual(404, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformDeleteRequest(secureDeleteUrl + id, headers);
            Assert.AreEqual(404, restResponse.StatusCode);
        }

        public void AddRecord(int id)
        {
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
        }
    }
}
