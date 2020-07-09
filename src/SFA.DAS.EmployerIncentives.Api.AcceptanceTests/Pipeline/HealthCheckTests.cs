using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Pipeline
{
    [TestFixture]
    [Parallelizable]
    public class WhenCheckingPipeLine
    {
        [Test]
        //[Ignore("being reworked")]
        public async Task CallPingEndpoint_ThenShouldReturnOkResponse()
        {
            var f = new HealthCheckFixture();
            var r =  await f.Client.GetAsync("/ping");
            r.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        //[Ignore("being reworked")]
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