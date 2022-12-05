using TechTalk.SpecFlow;

namespace SFA.DAS.Funding.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "innerApi")]
    public class InnerApi
    {
        private readonly TestContext _context;

        public InnerApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void Initialise()
        {
            if(_context.InnerApi == null)
                _context.InnerApi = new MockApi();
        }
    }
}
