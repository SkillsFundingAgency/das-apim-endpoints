using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests.Bindings
{
    [Binding]
    public class OuterApi
    {
        public static ApprenticePortalOuterApi Client { get; set; }
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
                    {"ApprenticeAccountsInnerApi:url", _context?.ApprenticeAccountsInnerApi?.BaseAddress + "/"},
                    {"ApprenticeCommitmentsInnerApi:url", _context?.ApprenticeCommitmentsInnerApi?.BaseAddress + "/"},
                    {"CommitmentsV2InnerApi:url", _context?.CommitmentsV2InnerApi?.BaseAddress + "/"},
                    {"TrainingProviderInnerApi:url", _context?.TrainingProviderInnerApi?.BaseAddress + "/"},
                    {"CoursesInnerApi:url", _context?.CoursesInnerApi?.BaseAddress + "/"},
                    {"AzureAD:tenant", ""},
                    {"AzureAD:identifier", ""}
                };

                Factory = new LocalWebApplicationFactory<Startup>(config, _context);
                Client = new ApprenticePortalOuterApi(Factory.CreateClient());
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
