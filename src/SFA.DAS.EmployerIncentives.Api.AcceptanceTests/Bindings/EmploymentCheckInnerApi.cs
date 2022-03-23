using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "employmentCheckInnerApi")]
    public class EmploymentCheckInnerApi
    {
        private readonly TestContext _context;

        public EmploymentCheckInnerApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void Initialise()
        {
            if(_context.EmploymentCheckApi == null)
                _context.EmploymentCheckApi = new MockApi();
        }
    }
}
