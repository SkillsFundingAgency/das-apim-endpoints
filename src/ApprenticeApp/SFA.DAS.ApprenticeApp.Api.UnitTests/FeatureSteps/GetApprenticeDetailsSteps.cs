using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Services;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeApp.Api.UnitTests.FeatureSteps
{
    [Binding]
    [Scope(Feature = "GetApprenticeDetails")]
    public class GetApprenticeDetailsSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private Apprentice _apprentice;
        private MyApprenticeship _myApprenticeship;
        private GetApprenticeApprenticeshipsResult _apprenticeshipsResult;
        private StandardApiResponse _standardCourse;
        private FrameworkApiResponse _frameworkCourse;

        public GetApprenticeDetailsSteps(TestContext context)
        {
            _context = context;
            _apprentice = _fixture.Create<Apprentice>();
            _apprenticeshipsResult = _fixture.Create<GetApprenticeApprenticeshipsResult>();
            _myApprenticeship = _fixture.Create<MyApprenticeship>();
            _standardCourse = _fixture.Create<StandardApiResponse>();
            _frameworkCourse = _fixture.Create<FrameworkApiResponse>();
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

        [Given(@"my apprenticeship exists")]
        public void GivenMyApprenticeshipExists()
        {
            _context.ApprenticeAccountsInnerApi.WithMyApprenticeship(_apprentice, _myApprenticeship);
        }

        [Given(@"my apprenticeship has a standard course")]
        public void GivenMyApprenticeshipHasAStandardCourse()
        {
            _context.CoursesInnerApi.WithStandardCourse(_myApprenticeship.StandardUId, _standardCourse);
        }

        [Given(@"my apprenticeship has a framework course")]
        public void GivenMyApprenticeshipHasAFrameworkCourse()
        {
            _myApprenticeship.StandardUId = null;
            _context.CoursesInnerApi.WithFrameworkCourse(_myApprenticeship.TrainingCode, _frameworkCourse);
        }

        [When(@"the apprentice's details are requested")]
        public async Task WhenTheApprenticeDetailsAreRequested()
        {
            await _context.OuterApiClient.Get($"/apprentices/{_apprentice.ApprenticeId}/details");
        }
        
        [Then(@"the result should contain the apprentice data, but with no apprenticeship data or my apprenticeship data")]
        public void ThenTheResultShouldContainTheApprenticeDataButWithNoApprenticeshipData()
        {
            var detailsModel = new ApprenticeDetails { Apprentice = _apprentice, MyApprenticeship = null};
            _context.OuterApiClient.Response.Should().Be200Ok().And.BeAs(detailsModel);
        }

        [Then(@"the result should have apprentice and first apprenticeship")]
        public void ThenTheResultShouldHaveApprenticeAndFirstApprenticeshipAndMyApprenticeship()
        {
            var detailsModel = new
            {
                Apprentice = _apprentice, MyApprenticeship = _apprenticeshipsResult.Apprenticeships.FirstOrDefault()
            };
            _context.OuterApiClient.Response.Should().BeEquivalentTo(detailsModel, o=>o.ExcludingMissingMembers());
        }

        [Then(@"a Framework MyApprenticeship course")]
        public void ThenAFrameworkMyApprenticeshipCourse()
        {
            _myApprenticeship.Title = _frameworkCourse.Title;
            _context.OuterApiClient.Response.Should().BeEquivalentTo(new { MyApprenticeship = _myApprenticeship},
                o => o.ExcludingMissingMembers());
        }

        [Then(@"a Standard MyApprenticeship course")]
        public void ThenAStandardMyApprenticeshipCourse()
        {
            _myApprenticeship.Title = _standardCourse.Title;
            _context.OuterApiClient.Response.Should().BeEquivalentTo(new { MyApprenticeship = _myApprenticeship },
                o => o.ExcludingMissingMembers());
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
