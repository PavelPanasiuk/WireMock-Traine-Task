﻿using Newtonsoft.Json;
using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace WireMock_Trainee
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = WireMockServer.Start();



            Console.WriteLine($"Server adress is {server.Urls[0]}");

            server.Given(Request.Create().WithPath("/test")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithBody("Hello from wiremock"));

            Console.ReadKey();
            server.Stop();
            server.Dispose();


        }



    }
}
