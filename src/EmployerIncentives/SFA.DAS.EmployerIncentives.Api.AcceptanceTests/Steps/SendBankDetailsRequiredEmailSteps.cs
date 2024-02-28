using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "SendBankDetailsRequiredEmail")]
    public class SendBankDetailsRequiredEmailSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture;
        private HttpResponseMessage _response;

        public SendBankDetailsRequiredEmailSteps(TestContext context)
        {
            _context = context;
            _fixture = new Fixture();
        }

        [Given(@"an employer is applying for the New Apprenticeship Incentive")]
        public void GivenAnEmployerIsApplyingForTheNewApprenticeshipIncentive()
        {

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/api/EmailCommand/bank-details-required")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(string.Empty)
                );
        }     

        [When(@"employer has indicated that they are unable to provide bank details at present")]
        public async Task WhenEmployerHasIndicatedThatTheyAreUnableToProvideBankDetailsAtPresent()
        {
            var request = new SendBankDetailsEmailRequest
            {
                AccountId = _fixture.Create<long>(),
                AccountLegalEntityId = _fixture.Create<long>(),
                EmailAddress = _fixture.Create<string>(),
                AddBankDetailsUrl = _fixture.Create<string>()
            };

            _response = await _context.OuterApiClient.PostAsync($"email/bank-details-required", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
            _response.EnsureSuccessStatusCode();
        }

        [Then(@"the employer is sent an email reminding them to supply their bank details")]
        public void ThenTheEmployerIsSentAnEmailRemindingThemToSupplyTheirBankDetails()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);

            var emailRequests = _context.InnerApi.MockServer.FindLogEntries(Request.Create().WithPath($"/api/EmailCommand/bank-details-required").UsingPost());
            emailRequests.ToList().Count().Should().Be(1);
        }

    }
}
