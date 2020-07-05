using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Pipeline
{
    [TestFixture]
    [Parallelizable]
    public class WhenCheckingPipeLine
    {

        [Test]
        public async Task WhenCall() { }


        [Test]
        public async Task CallPingEndpoint_ThenShouldReturnOkResponse()
        {
            var f = new HealthCheckFixture();
            var r =  await f.Client.GetAsync("/ping");
            r.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task CallHealthEndpoint_ThenShouldReturnOkResponse()
        {
            var f = new HealthCheckFixture();
            var r = await f.Client.GetAsync("/health");
            r.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }

    public class HealthCheckFixture
    {
        public readonly WebApplicationFactory<Startup> Factory;
        public readonly HttpClient Client;


        public HealthCheckFixture()
        {
            Factory = new CustomWebApplicationFactory<Startup>();

            Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
    }
}