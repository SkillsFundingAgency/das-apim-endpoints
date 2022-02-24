using AutoFixture;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;

namespace SFA.DAS.EmploymentCheck.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "RegisterCheckSuccess")]
    public class RegisterCheckSuccessSteps : RegisterCheckStepsBase
    {
        public RegisterCheckSuccessSteps(TestContext context) : base(context) { }

        [When(@"the Employer Incentives service are checking employment status of the apprentice")]
        public async Task WhenTheEmployerIncentivesServiceAreCheckingEmploymentStatusOfTheApprentice()
        {
            dynamic check = new
            {
                CorrelationId = Guid.NewGuid(),
                CheckType = Fixture.Create<string>(),
                Uln = Fixture.Create<long>(),
                ApprenticeshipAccountId = Fixture.Create<long>(),
                ApprenticeshipId = Fixture.Create<long>(),
                MinDate = DateTime.Now.AddDays(-100),
                MaxDate = DateTime.Now.AddDays(-90)
            };

            ExpectedResponseBody = $"{{\"errorType\":null,\"errorMessage\":null}}";

            Context.InnerApi?.MockServer
                .Given(
                    Request.Create()
                        .WithPath(Url)
                        .UsingPost()
                )
                .RespondWith(
                    WireMock.ResponseBuilders.Response.Create()
                        .WithBody(ExpectedResponseBody)
                        .WithStatusCode((int) HttpStatusCode.OK));

            if (Context.OuterApiClient != null)
            {
                Response = await Context.OuterApiClient.PostAsync(Url,
                    new StringContent(JsonSerializer.Serialize(check), Encoding.UTF8, "application/json"));
            }
        }

        [Then(@"a new Employment Check request is registered in Employment Check system")]
        public void ThenANewEmploymentCheckRequestIsRegisteredInEmploymentCheckSystem()
        {
            Response.Should().NotBeNull();
            Response?.EnsureSuccessStatusCode();
            Response?.Content.ReadAsStringAsync().Result.Should().Be(ExpectedResponseBody);
        }

    }
}
