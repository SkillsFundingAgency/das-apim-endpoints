using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticePortal.Models;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests.FeatureSteps
{
    [Binding]
    [Scope(Feature = "GetApprenticeHomePage")]
    public class GetApprenticeHomePageSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private Apprentice _apprentice;
        private GetApprenticeApprenticeshipsResult _apprenticeshipsResult;

        public GetApprenticeHomePageSteps(TestContext context)
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
        
        [Given(@"there is no apprenticeship")]
        public void GivenThereIsNoApprenticeship()
        {
        }

        [Given(@"there is no apprentice")]
        public void GivenThereIsNoApprentice()
        {
        }

        [Given(@"several apprenticeships")]
        public void GivenSeveralApprenticeships()
        {
            _context.ApprenticeCommitmentsInnerApi.WithApprenticeshipsResponseForApprentice(_apprentice, _apprenticeshipsResult);
        }

        [When(@"the apprentice's homepage is requested")]
        public async Task WhenTheApprenticeSHomepageIsRequested()
        {
            await _context.OuterApiClient.Get($"/apprentices/{_apprentice.ApprenticeId}/homepage");
        }
        
        [Then(@"the result should contain the apprentice data, but with no apprenticeship data")]
        public void ThenTheResultShouldContainTheApprenticeDataButWithNoApprenticeshipData()
        {
            var homePageModel = new ApprenticeHomepage {Apprentice = _apprentice, Apprenticeship = null};
            _context.OuterApiClient.Response.Should().Be200Ok().And.BeAs(homePageModel);
        }

        [Then(@"the result should have apprentice and first apprenticeship")]
        public void ThenTheResultShouldHaveApprenticeAndFirstApprenticeship()
        {
            var homePageModel = new ApprenticeHomepage { Apprentice = _apprentice, Apprenticeship = _apprenticeshipsResult.Apprenticeships.FirstOrDefault() };
            _context.OuterApiClient.Response.Should().Be200Ok().And.BeAs(homePageModel);
        }

        [Then(@"no apprenticeship data")]
        public void ThenNoApprenticeshipData()
        {
        }

        [Then(@"the result should be NotFound")]
        public void ThenTheResultShouldBeNotFound()
        {
            _context.OuterApiClient.Response.Should().Be404NotFound();
        }
    }
}
