using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "customerEngagementFinanceInnerApi")]
    public class CustomerEngagementFinanceInnerApi
    {
        private readonly TestContext _context;

        public CustomerEngagementFinanceInnerApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void Initialise()
        {
            _context.FinanceApiV1 = new MockApi();
        }
    }
}
