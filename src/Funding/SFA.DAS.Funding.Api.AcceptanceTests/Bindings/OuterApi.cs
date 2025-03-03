﻿using System.Collections.Generic;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace SFA.DAS.Funding.Api.AcceptanceTests.Bindings
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
            if (Client == null)
            {
                var config = new Dictionary<string, string>
                {
                    {"Environment", "LOCAL_ACCEPTANCE_TESTS"},
                    {"FundingApprenticeshipEarningsInnerApi:url", _context?.InnerApi?.BaseAddress + "/"},
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
