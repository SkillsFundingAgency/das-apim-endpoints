using TechTalk.SpecFlow;

namespace SFA.DAS.Funding.Api.AcceptanceTests.Bindings
{
    [Binding]
    public class TestCleanUp
    {
        [AfterTestRun()]
        public static void CleanUp()
        {
            MockServers.InnerApi?.Dispose();

            OuterApi.Factory?.Dispose();
            OuterApi.Client?.Dispose();

            OuterApi.Factory = null;
            OuterApi.Client = null;

            MockServers.InnerApi = null;
        }
    }
}
