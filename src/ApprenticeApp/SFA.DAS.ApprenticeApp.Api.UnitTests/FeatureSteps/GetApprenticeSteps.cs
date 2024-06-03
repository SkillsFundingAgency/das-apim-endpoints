using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeApp.Models;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeApp.Api.UnitTests.FeatureSteps
{
    [Binding]
    [Scope(Feature = "GetApprentice")]
    public class GetApprenticeSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private Apprentice _apprentice;
        private GetApprenticeApprenticeshipsResult _apprenticeshipsResult;

        public GetApprenticeSteps(TestContext context)
        {
            _context = context;
            _apprentice = _fixture.Create<Apprentice>();
            _apprenticeshipsResult = _fixture.Create<GetApprenticeApprenticeshipsResult>();
        }

        [Given(@"there is an apprentice")]
        public void GivenThereIsAnApprentice()
        {
            _context.ApprenticeAccountsInnerApi.WithApprentice(_apprentice);
        }
        
        [Given(@"there is no apprentice")]
        public void GivenThereIsNoApprentice()
        {
        }

        [When(@"the apprentice is requested")]
        public async Task WhenTheApprenticeIsRequested()
        {
            await _context.OuterApiClient.Get($"/apprentices/{_apprentice.ApprenticeId}");
        }
        
        [Then(@"the result should contain the apprentice data")]
        public void ThenTheResultShouldContainTheApprenticeData()
        {
            _context.OuterApiClient.Response.Should().Be200Ok().And.BeAs(_apprentice);
        }

        [Then(@"the result should be NotFound")]
        public void ThenTheResultShouldBeNotFound()
        {
            _context.OuterApiClient.Response.Should().Be404NotFound();
        }
    }
}
