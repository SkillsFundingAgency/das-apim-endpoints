using SFA.DAS.Earnings.Api.AcceptanceTests.Models;
using TechTalk.SpecFlow;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Bindings;

[Binding]
public class ModelInitialisation(ScenarioContext scenarioContext)
{
    [BeforeScenario(Order = 3)]
    public void Initialise()
    {
        var apprenticeship = new ApprenticeshipModel();
        scenarioContext.Set(apprenticeship);
    }
}

