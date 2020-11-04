using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "customerEngagementFinanceApi")]
    public class CustomerEngagementFinanceApi
    {
        private readonly TestContext _context;

        public CustomerEngagementFinanceApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void Initialise()
        {
            if(_context.FinanceApi == null)
                _context.FinanceApi = new MockApi();
        }
    }
}
