using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using Reqnroll;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Bindings;

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