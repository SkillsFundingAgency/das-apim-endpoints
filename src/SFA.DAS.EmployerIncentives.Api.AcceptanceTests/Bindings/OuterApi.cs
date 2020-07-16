using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Configuration;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "outerApi")]
    public class OuterApi 
    {
        private readonly TestContext _context;

        public OuterApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario()]
        public void InitialiseOuterApi()
        {

            var config = new Dictionary<string, string>();

            config.Add($"{EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration}:url", _context?.InnerApi?.BaseAddress);
            config.Add($"{EmployerIncentivesConfigurationKeys.CommitmentsV2InnerApiConfiguration}:url", _context?.CommitmentsV2InnerApi?.BaseAddress);

            var factory = new LocalWebApplicationFactory<Startup>(config);

            _context.OuterApiClient = factory.CreateClient();
        }
    }
}
