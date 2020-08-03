using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    public class ApplicationConfirmation
    {
        private readonly Fixture _fixture;
        private readonly TestContext _context;
        private ConfirmApplicationRequest _request;
        private HttpResponseMessage _response;

        public ApplicationConfirmation(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"an employer applying for a grant is asked to agree a declaration")]
        public void GivenAnEmployerApplyingForAGrantIsAskedToAgreeADeclaration()
        {
            _request = _fixture.Create<ConfirmApplicationRequest>();
        }

        [When(@"the employer understands and confirms the declaration")]
        public async Task WhenTheEmployerUnderstandsAndConfirmsTheDeclaration()
        {
            const string requestUri = @"/applications/confirm";
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath(requestUri)
                        .UsingAnyMethod()
                        // .WithBody(new JsonMatcher(JsonSerializer.Serialize(_request), true))
                        //    .UsingPatch())
                        )
                .RespondWith(
                    Response.Create()   
                        .WithStatusCode(HttpStatusCode.OK)
                );

            _response = await _context.OuterApiClient.PostAsJsonAsync(requestUri, _request);
        }


        [Then(@"then the employer application declaration is accepted")]
        public void ThenThenTheEmployerApplicationDeclarationIsAccepted()
        {
            Assert.True(_response.IsSuccessStatusCode);
        }
    }
}
