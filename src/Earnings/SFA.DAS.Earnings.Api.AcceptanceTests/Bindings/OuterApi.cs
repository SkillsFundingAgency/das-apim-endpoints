using TechTalk.SpecFlow;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "outerApi")]
    public class OuterApi
    {
        public static HttpClient Client { get; set; }
        public static LocalWebApplicationFactory<Startup> Factory { get; set; }

        private readonly TestContext _context;

        public OuterApi(TestContext context)
        {
            _context = context;
        }
        
        [BeforeScenario()]
        public void Initialise()
        {
            NUnit.Framework.TestContext.WriteLine("Initialising...");

            if (Client == null)
            {
                var config = new Dictionary<string, string>
                {
                    {"Environment", "LOCAL_ACCEPTANCE_TESTS"},
                    {"EarningsApiConfiguration:url", _context?.EarningsApi?.BaseAddress + "/"},
                    {"ApprenticeshipsApiConfiguration:url", _context?.ApprenticeshipsApi?.BaseAddress + "/"},
                    {"CollectionCalendarApiConfiguration:url", _context?.CollectionCalendarApi?.BaseAddress + "/"},
                    {"AzureAD:tenant", ""},
                    {"AzureAD:identifier", ""}
                };


                Factory = new LocalWebApplicationFactory<Startup>(config);
                Client = Factory.CreateClient();
            }

            _context.OuterApiClient = Client;
        }
    }
}
