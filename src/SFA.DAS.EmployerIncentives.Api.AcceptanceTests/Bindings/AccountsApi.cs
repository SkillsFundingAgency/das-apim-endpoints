using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "accountsApi")]
    public class AccountsApi
    {
        private readonly TestContext _context;

        public AccountsApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void Initialise()
        {
            if (_context.AccountsApi == null)
                _context.AccountsApi = new MockApi();
        }
    }
}
