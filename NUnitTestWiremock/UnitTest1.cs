using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;


namespace NUnitTestWiremock
{
    [TestFixture]
    public class Tests
    {
        WireMockServer server = WireMockServer.Start("https://www.google.com/");
        string _mockURL;

        [OneTimeSetUp]
        public void Setup()
        {
            server.Given(Request.Create().WithPath("/test")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithBodyAsJson("WireMock responce.")
                .WithStatusCode(HttpStatusCode.OK));
            _mockURL = server.Urls[0] + "/test";
        }

        [Test]
        public void TestStatusCode()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_mockURL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void TestResponseBody()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_mockURL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Assert.AreEqual("\"WireMock responce.\"", new StreamReader(response.GetResponseStream()).ReadToEnd());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            server.Stop();
            server.Dispose();
        }
    }
}