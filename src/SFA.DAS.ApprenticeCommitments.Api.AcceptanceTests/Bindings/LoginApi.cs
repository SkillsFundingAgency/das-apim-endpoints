using SFA.DAS.ApprenticeCommitments.Configuration;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "loginApi")]
    public class LoginApi
    {
        public static MockApi Client { get; set; }

        private readonly TestContext _context;

        public LoginApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void Initialise()
        { 
            Client??= new MockApi();
            _context.LoginApi = Client;
            _context.LoginConfig = new ApprenticeLoginConfiguration
            {
                CallbackUrl = "http://loginapi/callback",
                RedirectUrl = "http://loginapi/redirectUrl",
                IdentityServerClientId = "ABCD",
                Url = Client.BaseAddress + "/"
            };
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
