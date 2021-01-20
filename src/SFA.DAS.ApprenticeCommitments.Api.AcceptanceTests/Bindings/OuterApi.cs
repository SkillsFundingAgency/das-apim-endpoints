using System.Collections.Generic;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "outerApi")]
    public class OuterApi
    {
        public static HttpClient Client { get; set; }

        private readonly TestContext _context;
        
        public OuterApi(TestContext context)
        {
            _context = context;
        }
        
        [BeforeScenario()]
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

                var factory = new LocalWebApplicationFactory<Startup>(config);
                Client = factory.CreateClient();
            }

            _context.OuterApiClient = Client;
        }
    }
}
