using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using TechTalk.SpecFlow;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps
{
    [Binding]
    public class WithdrawalSteps(TestContext testContext, ScenarioContext scenarioContext)
    {
        [Given(@"the learner withdraws on (.*)")]
        public void GivenTheLearnerWithdrawsOnWithdrawal_Date(DateTime withdrawalDate)
        {
            var apprenticeship = scenarioContext.Get<ApprenticeshipModel>();
            apprenticeship.WithdrawnDate = withdrawalDate;
        }
    }
}
