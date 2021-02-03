using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "commitmentsV2InnerApi")]
    public class CommitmentsV2InnerApi
    {
        public static MockApi Client { get; set; }

        private readonly TestContext _context;

        public CommitmentsV2InnerApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void Initialise()
        {
            Client ??= new MockApi();
            _context.CommitmentsV2InnerApi = Client;
        }

        [AfterScenario()]
        public static void CleanUp()
        {
            Client?.Reset();
        }

        [AfterFeature()]
        public static void CleanUpFeature()
        {
            Client?.Dispose();
            Client = null;
        }
    }
}