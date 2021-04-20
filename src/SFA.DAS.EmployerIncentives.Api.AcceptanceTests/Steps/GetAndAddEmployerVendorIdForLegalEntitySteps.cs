using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Data;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
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
    [Scope(Feature = "GetAndAddEmployerVendorIdForLegalEntity")]
    public class GetAndAddEmployerVendorIdForLegalEntitySteps
    {
        private readonly TestContext _context;
        private const string HashedLegalEntityId = "DW5T8V";
        private const string ExpectedEmployerVendorId = "P0004320";

        public GetAndAddEmployerVendorIdForLegalEntitySteps(TestContext context) { _context = context; }

        [When(@"Get and Add Employer Vendor Id is invoked")]
        public void WhenGetAndAddEmployerVendorIdIsInvoked()
        {
        }

        [Then(@"employer vendor Id is returned from the Finance API")]
        public async Task ThenEmployerVendorIdIsReturnedFromTheFinanceAPI()
        {
            await GetResponseFromFinanceApi();

            _context.FinanceApi.MockServer.FindLogEntries(Request.Create()
                .WithPath($"/Finance/{_context.FinanceApi.CompanyName}/vendor/aleid={HashedLegalEntityId}")
                .WithParam("api-version", _context.FinanceApi.ApiVersion)
                .UsingGet()).Should().HaveCount(1);
        }

        [Then(@"the legal entity is sent an update of the employer vendor Id")]
        public void ThenTheLegalEntityIsSentAnUpdateOfTheEmployerVendorId()
        {
            _context.InnerApi.MockServer.FindLogEntries(Request.Create()
                .WithPath(p => p.Contains($"legalentities/{HashedLegalEntityId}/employervendorid"))
                .WithBody(CheckBody)
                .UsingPut()).Should().HaveCount(1);
        }

        [Then(@"employer vendor Id is not returned from the Finance API")]
        public async Task ThenEmployerVendorIdIsNotReturnedFromTheFinanceAPI()
        {
            await GetErrorResponseFromFinanceApi();

            _context.FinanceApi.MockServer.FindLogEntries(Request.Create()
                .WithPath($"/Finance/{_context.FinanceApi.CompanyName}/vendor/aleid={HashedLegalEntityId}")
                .WithParam("api-version", _context.FinanceApi.ApiVersion)
                .UsingGet()).Should().HaveCount(1);
        }

        [Then(@"the legal entity is not sent an update of the employer vendor Id")]
        public void ThenTheLegalEntityIsNotSentAnUpdateOfTheEmployerVendorId()
        {
            _context.InnerApi.MockServer.FindLogEntries(Request.Create()
                .WithPath(p => p.Contains($"legalentities/{HashedLegalEntityId}/employervendorid"))
                .UsingPut()).Should().HaveCount(0);
        }

        private static bool CheckBody(string json)
        {
            var body = JsonConvert.DeserializeObject<PutEmployerVendorIdForLegalEntityRequestData>(json);
            return body.EmployerVendorId == ExpectedEmployerVendorId;
        }

        private async Task GetResponseFromFinanceApi()
        {
            _context.FinanceApi.MockServer
                .Given(ExpectedGetVendorDataApiRequest)
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(TestData.FinanceAPI_V1__VendorDataResponse)
                );

            var url = $"legalentities/{HashedLegalEntityId}/employervendorid";
            await _context.OuterApiClient.PutAsync(url, new StringContent(""));
        }

        private async Task GetErrorResponseFromFinanceApi()
        {
            _context.FinanceApi.MockServer
                .Given(ExpectedGetVendorDataApiRequest)
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(TestData.FinanceAPI_V1__VendorDataResponse_Error)
                );


            var url = $"legalentities/{HashedLegalEntityId}/employervendorid";
            await _context.OuterApiClient.PutAsync(url, new StringContent(""));
        }

        public IRequestMatcher ExpectedGetVendorDataApiRequest =>

            Request.Create()
                .WithPath($"/Finance/{_context.FinanceApi.CompanyName}/vendor/aleid={HashedLegalEntityId}")
                .WithParam("api-version", _context.FinanceApi.ApiVersion)
                .UsingGet();
    }
}
