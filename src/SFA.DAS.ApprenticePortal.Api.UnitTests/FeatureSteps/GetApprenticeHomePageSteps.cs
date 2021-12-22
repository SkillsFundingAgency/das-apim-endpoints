using System;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests.FeatureSteps
{
    [Binding]
    public class GetApprenticeHomePageSteps
    {
        [Given(@"there is an apprentice")]
        public void GivenThereIsAnApprentice()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"there is no apprenticeship")]
        public void GivenThereIsNoApprenticeship()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the apprentice is requested")]
        public void WhenTheApprenticeIsRequested()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should contain the apprentice data")]
        public void ThenTheResultShouldContainTheApprenticeData()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should be NotFound")]
        public void ThenTheResultShouldBeNotFound()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
