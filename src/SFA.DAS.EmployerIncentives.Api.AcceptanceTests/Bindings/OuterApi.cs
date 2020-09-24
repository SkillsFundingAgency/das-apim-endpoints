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

            var config = new Dictionary<string, string>
            {
                {"Environment", "LOCAL_ACCEPTANCE_TESTS"},
                {"EmployerIncentivesInnerApi:url", _context?.InnerApi?.BaseAddress + "/"},
                {"CommitmentsV2InnerApi:url", _context?.CommitmentsV2InnerApi?.BaseAddress + "/"},
                {"CustomerEngagementFinanceInnerApi:url", _context?.FinanceApiV1?.BaseAddress + "/"},
                {"AzureAD:tenant", ""},
                {"AzureAD:identifier", ""}
            };


            var factory = new LocalWebApplicationFactory<Startup>(config);

            _context.OuterApiClient = factory.CreateClient();
            _context.Factory = factory;
        }
    }
}
