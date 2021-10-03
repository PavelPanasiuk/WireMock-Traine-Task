using NUnit.Framework;
using System.IO;
using System.Net;
using System.Net.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using System;
using System.Threading.Tasks;

namespace NUnitTestWiremock
{
    [TestFixture]
    public class WiremockUnitTest
    {
        WireMockServer _server = WireMockServer.Start();
        string _mockURL;

        [OneTimeSetUp]
        public void Setup()
        {
            _server.Given(Request.Create().WithPath("/test")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson("WireMock responce.")
                .WithStatusCode(HttpStatusCode.OK));
            _mockURL = _server.Urls[0] + "/test";
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

        [Test]
        public async Task IncorrectApiMethodAsync()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_mockURL);
            request.Method = HttpMethod.Post;

            HttpResponseMessage response = await client.SendAsync(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _server.Stop();
        }
    }
}