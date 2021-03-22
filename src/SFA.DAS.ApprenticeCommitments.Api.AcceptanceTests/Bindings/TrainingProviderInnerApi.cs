using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "trainingProviderInnerApi")]
    public class TrainingProviderInnerApi
    {
        public static MockApi Client { get; set; }

        private readonly TestContext _context;

        public TrainingProviderInnerApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void Initialise()
        {
            Client ??= new MockApi();
            _context.TrainingProviderInnerApi = Client;
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