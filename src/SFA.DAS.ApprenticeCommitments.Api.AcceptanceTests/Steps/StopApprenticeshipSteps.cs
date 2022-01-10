using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "StopApprenticeship")]
    public class StopApprenticeshipSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private StopApprovalCommand _request;

        public StopApprenticeshipSteps(TestContext context)
        {
            _context = context;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/approvals/stopped")
                        .UsingPost()
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                            );
        }

        [When("an apprenticeship stop is posted")]
        public async Task WhenTheFollowingApprenticeshipIsPosted()
        {
            _request = _fixture.Create<StopApprovalCommand>();
            await _context.OuterApiClient.Post("approvals/stopped", _request);
        }

        [Then("the response should be OK")]
        public void ThenTheResponseShouldBeOK()
        {
            _context.OuterApiClient.Response.Should().Be2XXSuccessful();
        }

        [Then("the inner API has received the posted values")]
        public void ThenTheRequestToTheInnerApiWasMappedCorrectly()
        {
            _context.InnerApi.SingleLogBody.Should().NotBeEmpty()
                .And.ShouldBeJson<StopApprovalCommand>()
                .Which.Should().BeEquivalentTo(_request);
        }
    }
}