using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.ParallelExecution
{
    [TestClass]
    public class TestParallelExecution
    {
        private string delayedGetUrl = "http://localhost:8080/laptop-bag/webapi/delay/all";
        private string delayedPostUrl = "http://localhost:8080/laptop-bag/webapi/delay/add";
        private string delayedPutUrl = "http://localhost:8080/laptop-bag/webapi/delay/update";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();
        private HttpClientAsyncHelper httpClientAsyncHelper = new HttpClientAsyncHelper();

        private void SendGetRequest()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");

            RestResponse restResponse = httpClientAsyncHelper.PerformGetRequest(delayedGetUrl, httpHeader).GetAwaiter()
                .GetResult();

            List<JsonRootObject> jsonData = ResponseDataHelper.DeserializeJsonResponse<List<JsonRootObject>>(restResponse.ResponseContent);

            Console.WriteLine(jsonData.ToString());
        }

        private void SendPostRequest()
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

            RestResponse restResponse =  httpClientAsyncHelper.PerformPostRequest(delayedPostUrl, xmlData, xmlMediaType, headers).GetAwaiter()
                .GetResult();

            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlDataLaptop = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Console.WriteLine(xmlDataLaptop);
        }

        private void SendPutRequest()
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

            RestResponse restResponse = httpClientAsyncHelper.PerformPostRequest(delayedPostUrl, xmlData, xmlMediaType, headers).GetAwaiter().GetResult();
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

            restResponse = httpClientAsyncHelper.PerformPutRequest(delayedPutUrl, xmlData, xmlMediaType, headers).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = httpClientAsyncHelper.PerformGetRequest(delayedGetUrl + id, headers).GetAwaiter()
                .GetResult();
            Laptop xmlDataLaptop = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsTrue(xmlDataLaptop.Features.Feature.Contains("1 TB of SSD"));
        }

        [TestMethod]
        public void TestTask()
        {
            Task get = new Task(() => SendGetRequest());
            get.Start();

            Task put = new Task(() => SendPutRequest());
            put.Start();

            Task post = new Task(() => SendPostRequest());
            post.Start();

            get.Wait();
            put.Wait();
            post.Wait();
        }
    }
}
