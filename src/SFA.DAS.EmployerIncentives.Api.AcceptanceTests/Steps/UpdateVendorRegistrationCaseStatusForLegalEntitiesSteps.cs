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
        private readonly DateTime _dateTimeTo = DateTime.SpecifyKind(new DateTime(2020, 9, 1, 10, 0, 0), DateTimeKind.Utc);
        private readonly string[] _hashedLegalEntitiesFromFinanceJson = new[] { "DW5T8V", "HEN123" };
        private HttpResponseMessage _response;

        public UpdateVendorRegistrationCaseStatusForLegalEntitiesSteps(TestContext context) { _context = context; }

        [When(@"Refresh Vendor Registration Form Status is invoked")]
        public async Task WhenRefreshVendorRegistrationFormStatusIsInvoked()
        {
            SetResponseFromFinanceApi(TestData.FinanceAPI_V1__VendorRegistrationCasesbyLastStatusChangeDate);
            SetupExpectedEmployerIncentivesApiCalls();

            var url = $"legalentities/vendorregistrationform/status?from={_dateTimeFrom:yyyy-MM-ddTHH:mm:ssZ}&to={_dateTimeTo:yyyy-MM-ddTHH:mm:ssZ}";
            _response = await _context.OuterApiClient.PatchAsync(url, new StringContent(""));
        }

        [Then(@"latest VRF cases are retrieved from Finance API")]
        public void ThenLatestVRFCasesAreRetrievedFromFinanceAPI()
        {
            _context.FinanceApiV1.MockServer.FindLogEntries(Request.Create()
                .WithPath("/Finance/Registrations")
                .UsingGet()).Should().HaveCount(1);
        }

        [Then(@"VRF case details are updated for legal entities")]
        public void ThenVRFCaseDetailsAreUpdatedForLegalEntities()
        {
            _response.EnsureSuccessStatusCode();

            _context.InnerApi.MockServer.FindLogEntries(Request.Create()
                .WithPath(p => p.Contains("/legalentities/"))
                .UsingPatch()).Should().HaveCount(2);
        }

        [When(@"Refresh Vendor Registration Form Status is invoked and New and other case types exist in VRF")]
        public async Task WhenRefreshVendorRegistrationFormStatusIsInvokedAndNewAndOtherCaseTypesExistInVRF()
        {
            SetResponseFromFinanceApi(TestData.FinanceAPI_V1_VendorRegistrationCasesWithMixedCaseType);
            SetupExpectedEmployerIncentivesApiCalls();

            var url = $"legalentities/vendorregistrationform/status?from={_dateTimeFrom:yyyy-MM-ddTHH:mm:ssZ}&to={_dateTimeTo:yyyy-MM-ddTHH:mm:ssZ}";
            _response = await _context.OuterApiClient.PatchAsync(url, new StringContent(""));
        }

        [Then(@"VRF case details are updated only for legal entities with a case type of New")]
        public void ThenVRFCaseDetailsAreUpdatedOnlyForLegalEntitiesWithACaseTypeOfNew()
        {
            _response.EnsureSuccessStatusCode();

            _context.InnerApi.MockServer.FindLogEntries(Request.Create()
                .WithPath(p => p.Contains("/legalentities/"))
                .UsingPatch()).Should().HaveCount(1);
        }

        private void SetResponseFromFinanceApi(byte[] testData)
        {
            _context.FinanceApiV1.MockServer
                .Given(ExpectedFinanceApiRequest)
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(testData)
                );
        }

        public IRequestMatcher ExpectedFinanceApiRequest =>

            Request.Create()
                .WithPath("/Finance/Registrations")
                .WithParam("DateTimeFrom", $"{_dateTimeFrom.ToLocalTime():yyyy-MM-ddTHH:mm:ssZ}")
                .WithParam("DateTimeTo", $"{_dateTimeTo.ToLocalTime():yyyy-MM-ddTHH:mm:ssZ}")
                .WithParam("VendorType", "EMPLOYER")
                .WithParam("api-version", "2019-06-01")
                .UsingGet();

        private void SetupExpectedEmployerIncentivesApiCalls()
        {

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/legalentities/{_hashedLegalEntitiesFromFinanceJson[0]}/vendorregistrationform/status")
                        .UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK));

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/legalentities/{_hashedLegalEntitiesFromFinanceJson[1]}/vendorregistrationform/status")
                        .UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK));
        }
    }
}
