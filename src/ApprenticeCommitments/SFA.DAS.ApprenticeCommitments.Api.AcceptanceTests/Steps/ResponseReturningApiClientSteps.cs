using FluentAssertions;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ResponseReturningApiClient")]
    public class ResponseReturningApiClientSteps
    {
        private readonly TestContext _context;
        private string _url;
        private string _request;

        public ResponseReturningApiClientSteps(TestContext context)
        {
            _context = context;

            _context.InnerApi.MockServer
                .Given(Request.Create()
                    .WithPath("*")
                    .UsingAnyMethod()
                ).RespondWith(Response.Create()
                    .WithStatusCode((int)HttpStatusCode.OK));
        }

        [When("the outer api recieves a request to '([^']*)' with body")]
        public async Task WhenTheFollowingApprenticeshipIsPosted(string url, string request)
        {
            _url = url;
            _request = CanonicalJson(request);
            await _context.OuterApiClient.Post(url, _request);
        }

        private static string CanonicalJson(string request)
            => JsonConvert.SerializeObject(JsonConvert.DeserializeObject(request));

        [Then("the outer api forwards the request to the inner api")]
        public void ThenTheResponseShouldBeOK()
        {
            _context.OuterApiClient.Response.Should().Be2XXSuccessful();
            _context.InnerApi.MockServer.LogEntries.Should().HaveCount(1);

            var req = _context.InnerApi.MockServer.FindLogEntries(Request.Create().WithPath(_url));
            req.First().RequestMessage.Body.Should().Be(_request);
        }
    }
}