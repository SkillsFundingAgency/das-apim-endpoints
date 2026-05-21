using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps.Fm36;

[Binding]
public class EnglishAndMathsSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    [Given(@"has the following sld englishAndMaths records")]
    public void GivenHasTheFollowingSldEnglishAndMathsRecords(Table table)
    {
        var apprenticeship = scenarioContext.Get<ApprenticeshipModel>();
        var englishAndMathsCourses = table.CreateSet<EnglishAndMathsModel>().ToList();
        apprenticeship.EnglishAndMaths = englishAndMathsCourses;
    }
}
