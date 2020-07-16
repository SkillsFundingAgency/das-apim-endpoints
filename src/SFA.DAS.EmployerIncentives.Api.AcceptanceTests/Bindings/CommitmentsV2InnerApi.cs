using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "commitmentsV2InnerApi")]
    public class CommitmentsV2InnerApi
    {
        private readonly TestContext _context;

        public CommitmentsV2InnerApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void InitialiseInnerApi()
        {
            _context.CommitmentsV2InnerApi = new MockApi();
        }
    }
}
