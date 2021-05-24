using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebServiceAutomation
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [Ignore]
        public void TestMethod1()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Dispose();
        }
    }
}
