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
    [Scope(Feature = "RegisterCheckFailure")]
    public class RegisterCheckFailureSteps : RegisterCheckStepsBase
    {
        public RegisterCheckFailureSteps(TestContext context) : base(context) { }

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

            ExpectedResponseBody =
                $"{{VersionId:null,ErrorType:{Fixture.Create<string>()},ErrorMessage:{Fixture.Create<string>()}}}";

            Context.InnerApi?.MockServer
                .Given(
                    Request.Create().WithPath(Url)
                        .UsingPost()
                )
                .RespondWith(
                    WireMock.ResponseBuilders.Response.Create()
                        .WithBody(ExpectedResponseBody)
                        .WithStatusCode((int) HttpStatusCode.BadRequest));

            if (Context.OuterApiClient != null)
            {
                Response = await Context.OuterApiClient.PostAsync(Url,
                    new StringContent(JsonSerializer.Serialize(check), Encoding.UTF8, "application/json"));
            }
        }

        [Then(@"an error response is returned by the Employment Check system")]
        public void ThenAnErrorResponseIsReturnedByTheEmploymentCheckSystem()
        {
            Response.Should().NotBeNull();
            Response?.IsSuccessStatusCode.Should().BeFalse();
            Response?.Content.ReadAsStringAsync().Result.Should().Be(ExpectedResponseBody);
        }
    }
}
