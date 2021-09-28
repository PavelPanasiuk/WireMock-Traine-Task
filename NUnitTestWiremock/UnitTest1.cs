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

        WireMockServer server = WireMockServer.Start();
        string _mockURL;

        [SetUp]
        public void Setup()
        {
            server.Given(Request.Create().WithPath("/test")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithBodyAsJson("WireMock responce.")
                .WithStatusCode(HttpStatusCode.OK));
            _mockURL = server.Urls[0]+"/test";

        }

        [Test]
        public void Test1()
        {


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_mockURL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }


            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            server.Stop();
            server.Dispose();
        }
    }
}