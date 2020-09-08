using System.Collections.Generic;
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
        public void Initialise()
        {

            var config = new Dictionary<string, string>();

            config.Add("Environment", "LOCAL_ACCEPTANCE_TESTS");
            config.Add("EmployerIncentivesInnerApi:url", _context?.InnerApi?.BaseAddress + "/");
            config.Add($"CommitmentsV2InnerApi:url", _context?.CommitmentsV2InnerApi?.BaseAddress + "/");
            config.Add("AzureAD:tenant", "");
            config.Add("AzureAD:identifier", "");

            var factory = new LocalWebApplicationFactory<Startup>(config);

            _context.OuterApiClient = factory.CreateClient();
            _context.Factory = factory;
        }
    }
}
