using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Data;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using TechTalk.SpecFlow;
using WireMock.Matchers.Request;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetAndAddEmployerVendorIdForLegalEntity")]
    public class GetAndAddEmployerVendorIdForLegalEntitySteps
    {
        private readonly TestContext _context;
        private readonly string _hashedLegalEntityId = "DW5T8V";
        private HttpResponseMessage _response;
        private const string CompanyName = "ESFA";

        public GetAndAddEmployerVendorIdForLegalEntitySteps(TestContext context) { _context = context; }


        [When(@"Get and Add Employer Vendor Id is invoked")]
        public async Task WhenGetAndAddEmployerVendorIdIsInvoked()
        {
            SetResponseFromFinanceApi();
            //    SetupExpectedEmployerIncentivesApiCalls();

            try
            {
                var url = $"legalentities/{_hashedLegalEntityId}/employervendorid";

                _response = await _context.OuterApiClient.PutAsync(url, new StringContent(""));
            }
            catch(Exception e)
            {
                var a = e;
            }
        }

        [Then(@"employer vendor Id is retrieved from the Finance API")]
        public void ThenEmployerVendorIdIsRetrievedFromTheFinanceAPI()
        {
            _context.FinanceApiV1.MockServer.FindLogEntries(Request.Create()
                    .WithPath($"/Finance/{CompanyName}/vendor/aleid={_hashedLegalEntityId}")
                    .WithParam("api-version", "2019-06-01")
                    .UsingGet()).Should().HaveCount(1);
        }

        [Then(@"the legal entity is sent an update of the employer vendor Id")]
        public void ThenTheLegalEntityIsSentAnUpdateOfTheEmployerVendorId()
        {
            _context.InnerApi.MockServer.FindLogEntries(Request.Create()
                .WithPath(p => p.Contains($"legalentities/{_hashedLegalEntityId}/employervendorid"))
                .WithBody(p=> CheckObject(p))
                .UsingPut()).Should().HaveCount(1);
        }

        private bool CheckObject(string bytes)
        {
            var r = bytes.Contains("P0004320");
            return r;
        }


        //[Then(@"latest VRF cases are retrieved from Finance API")]
        //public void ThenLatestVRFCasesAreRetrievedFromFinanceAPI()
        //{
        //    _context.FinanceApiV1.MockServer.FindLogEntries(Request.Create()
        //        .WithPath("/Finance/Registrations")
        //        .UsingGet()).Should().HaveCount(1);
        //}

        //[Then(@"VRF case details are updated for legal entities")]
        //public void ThenVRFCaseDetailsAreUpdatedForLegalEntities()
        //{
        //    _response.EnsureSuccessStatusCode();

        //    _context.InnerApi.MockServer.FindLogEntries(Request.Create()
        //        .WithPath(p => p.Contains("/legalentities/"))
        //        .UsingPatch()).Should().HaveCount(2);
        //}

        private void SetResponseFromFinanceApi()
        {
            _context.FinanceApiV1.MockServer
                .Given(ExpectedGetVendorDataApiRequest)
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(TestData.FinanceAPI_V1__VendorDataResponse)
                );
        }

        public IRequestMatcher ExpectedGetVendorDataApiRequest =>

            Request.Create()
                .WithPath($"/Finance/{CompanyName}/vendor/aleid={_hashedLegalEntityId}")
                .WithParam("api-version", "2019-06-01")
                .UsingGet();

        //private void SetupExpectedEmployerIncentivesApiCalls()
        //{

        //    _context.InnerApi.MockServer
        //        .Given(
        //            Request.Create().WithPath($"/legalentities/{_hashedLegalEntitiesFromFinanceJson[0]}/vendorregistrationform/status")
        //                .UsingPatch())
        //        .RespondWith(
        //            Response.Create()
        //                .WithStatusCode((int)HttpStatusCode.OK));

        //    _context.InnerApi.MockServer
        //        .Given(
        //            Request.Create().WithPath($"/legalentities/{_hashedLegalEntitiesFromFinanceJson[1]}/vendorregistrationform/status")
        //                .UsingPatch())
        //        .RespondWith(
        //            Response.Create()
        //                .WithStatusCode((int)HttpStatusCode.OK));
        //}
    }
}
