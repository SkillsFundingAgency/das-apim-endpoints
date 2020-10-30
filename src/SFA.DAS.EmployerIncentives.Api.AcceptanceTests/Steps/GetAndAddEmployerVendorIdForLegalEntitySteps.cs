using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
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
        private const string ExpectedEmployerVendorId = "P0004320";

        public GetAndAddEmployerVendorIdForLegalEntitySteps(TestContext context) { _context = context; }


        [When(@"Get and Add Employer Vendor Id is invoked")]
        public async Task WhenGetAndAddEmployerVendorIdIsInvoked()
        {
            SetResponseFromFinanceApi();

            var url = $"legalentities/{_hashedLegalEntityId}/employervendorid";
            _response = await _context.OuterApiClient.PutAsync(url, new StringContent(""));
        }

        [Then(@"employer vendor Id is retrieved from the Finance API")]
        public void ThenEmployerVendorIdIsRetrievedFromTheFinanceAPI()
        {
            _context.FinanceApi.MockServer.FindLogEntries(Request.Create()
                    .WithPath($"/Finance/{CompanyName}/vendor/aleid={_hashedLegalEntityId}")
                    .WithParam("api-version", "2019-06-01")
                    .UsingGet()).Should().HaveCount(1);
        }

        [Then(@"the legal entity is sent an update of the employer vendor Id")]
        public void ThenTheLegalEntityIsSentAnUpdateOfTheEmployerVendorId()
        {
            _context.InnerApi.MockServer.FindLogEntries(Request.Create()
                .WithPath(p => p.Contains($"legalentities/{_hashedLegalEntityId}/employervendorid"))
                .WithBody(p=> CheckBody(p))
                .UsingPut()).Should().HaveCount(1);
        }

        private bool CheckBody(string json)
        {
            var body = JsonConvert.DeserializeObject<PutEmployerVendorIdForLegalEntityRequestData>(json);
            return body.EmployerVendorId == ExpectedEmployerVendorId;
        }

        private void SetResponseFromFinanceApi()
        {
            _context.FinanceApi.MockServer
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
    }
}
