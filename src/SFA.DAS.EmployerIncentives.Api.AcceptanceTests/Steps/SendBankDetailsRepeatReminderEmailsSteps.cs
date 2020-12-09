using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.Models;
using System;
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
    [Scope(Feature = "SendBankDetailsRepeatReminderEmails")]
    public class SendBankDetailsRepeatReminderEmailsSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture;
        private HttpResponseMessage _response;

        public SendBankDetailsRepeatReminderEmailsSteps(TestContext context)
        {
            _context = context;
            _fixture = new Fixture();
        }

        [Given(@"an employer has applied for the employer incentive")]
        public void GivenAnEmployerHasAppliedForTheEmployerIncentive()
        {
            _context.InnerApi.MockServer
               .Given(
                   Request.Create().WithPath($"/api/EmailCommand/bank-details-repeat-reminders")
                       .UsingPost())
               .RespondWith(
                   Response.Create()
                       .WithStatusCode((int)HttpStatusCode.OK)
                       .WithHeader("Content-Type", "application/json")
                       .WithBody(string.Empty)
               );
        }

        [When(@"employer has not supplied their bank details")]
        public async Task WhenEmployerHasNotSuppliedTheirBankDetails()
        {
            var request = new BankDetailsRepeatReminderEmailsRequest
            {
               ApplicationCutOffDate = _fixture.Create<DateTime>()
            };

            _response = await _context.OuterApiClient.PostAsync($"email/bank-details-repeat-reminders", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
            _response.EnsureSuccessStatusCode();
        }

        [Then(@"the employer is sent an email reminding them how to supply their bank details so that they can receive payment")]
        public void ThenTheEmployerIsSentAnEmailRemindingThemHowToSupplyTheirBankDetailsSoThatTheyCanReceivePayment()
        {
            _context.InnerApi.MockServer
               .Given(
                   Request.Create().WithPath($"/api/EmailCommand/bank-details-repeat-reminders")
                       .UsingPost())
               .RespondWith(
                   Response.Create()
                       .WithStatusCode((int)HttpStatusCode.OK)
                       .WithHeader("Content-Type", "application/json")
                       .WithBody(string.Empty)
               );
        }

    }
}
