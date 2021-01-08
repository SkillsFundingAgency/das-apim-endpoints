using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.Matchers.Request;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    public class UpdateVendorRegistrationCaseStatusForLegalEntitiesSteps
    {
        private readonly TestContext _context;
        private readonly DateTime _dateTimeFrom = DateTime.SpecifyKind(new DateTime(2020, 9, 1, 9, 0, 0), DateTimeKind.Utc);
        private readonly string[] _hashedLegalEntitiesFromFinanceJson = { "DW5T8V", "HEN123" };
        private HttpResponseMessage _response;

        public UpdateVendorRegistrationCaseStatusForLegalEntitiesSteps(TestContext context) { _context = context; }

        [When(@"Refresh Vendor Registration Form Status is invoked")]
        public async Task WhenRefreshVendorRegistrationFormStatusIsInvoked()
        {
            SetResponseFromFinanceApi();
            SetupExpectedEmployerIncentivesApiCalls();

            const string url = "legalentities/vendorregistrationform";
            _response = await _context.OuterApiClient.PatchAsync(url, new StringContent(""));
        }

        [Then(@"VRF case updates are retrieved from Finance API")]
        public void ThenLatestVrfCasesAreRetrievedFromFinanceApi()
        {
            _context.FinanceApi.MockServer.FindLogEntries(Request.Create()
                .WithPath("/Finance/Registrations")
                .UsingGet()).Should().HaveCount(1);
        }

        [Then(@"the latest update date is retrieved from Employer Incentives API")]
        public void ThenTheLatestUpdateDateIsRetrievedFromEmployerIncentivesApi()
        {
            _context.InnerApi.MockServer.FindLogEntries(Request.Create()
                .WithPath("/accounts/last-vrf-update-date")
                .UsingGet()).Should().HaveCount(1);
        }


        [Then(@"VRF case details are updated for legal entities")]
        public void ThenVrfCaseDetailsAreUpdatedForLegalEntities()
        {
            _response.EnsureSuccessStatusCode();

            _context.InnerApi.MockServer.FindLogEntries(Request.Create()
                .WithPath(p => p.Contains("/legalentities/"))
                .UsingPatch()).Should().HaveCount(4);
        }

        private void SetResponseFromFinanceApi()
        {
            _context.FinanceApi.MockServer
                .Given(ExpectedFinanceApiRequest)
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(TestData.FinanceAPI_V1__VendorRegistrationCasesbyLastStatusChangeDate)
                );
        }

        public IRequestMatcher ExpectedFinanceApiRequest =>

            Request.Create()
                .WithPath("/Finance/Registrations")
                .WithParam("DateTimeFrom", $"{_dateTimeFrom:yyyy-MM-ddTHH:mm:ssZ}")
                .WithParam("VendorType", "EMPLOYER")
                .WithParam("api-version", "2019-06-01")
                .UsingGet();

        private void SetupExpectedEmployerIncentivesApiCalls()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath("/accounts/last-vrf-update-date")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithBodyAsJson(new { LastUpdateDateTime = _dateTimeFrom })
                        .WithStatusCode((int)HttpStatusCode.OK));

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/legalentities/{_hashedLegalEntitiesFromFinanceJson[0]}/vendorregistrationform")
                        .UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK));

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/legalentities/{_hashedLegalEntitiesFromFinanceJson[1]}/vendorregistrationform")
                        .UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK));
        }
    }
}
