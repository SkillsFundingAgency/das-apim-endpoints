using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Bindings
{
    [Binding]
    public class OuterApi
    {
        public static ApprenticeCommitmentsApi Client { get; set; }
        public static LocalWebApplicationFactory<Startup> Factory { get; set; }

        private readonly TestContext _context;
        
        public OuterApi(TestContext context)
        {
            _context = context;
        }
        
        [BeforeScenario(Order = 10)]
        public void Initialise()
        {
            if (Client == null)
            {
                var config = new Dictionary<string, string>
                {
                    {"Environment", "LOCAL_ACCEPTANCE_TESTS"},
                    {"ApprenticeCommitmentsInnerApi:url", _context?.InnerApi?.BaseAddress + "/"},
                    {"AzureAD:tenant", ""},
                    {"AzureAD:identifier", ""}
                };

                Factory = new LocalWebApplicationFactory<Startup>(config, _context);
                Client = new ApprenticeCommitmentsApi(Factory.CreateClient());
            }

            _context.OuterApiClient = Client;
        }

        [AfterFeature()]
        public static void CleanUpFeature()
        {
            Client?.Dispose();
            Client = null;
            Factory?.Dispose();
            Factory = null;
        }
    }
}
