using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Bindings
{
    [Binding]
    public class TestCleanUp
    {
        [AfterTestRun()]
        public static void CleanUp()
        {
            MockServers.InnerApi?.Dispose();
            MockServers.AccountsApi?.Dispose();
            MockServers.CommitmentsV2InnerApi?.Dispose();
            MockServers.FinanceApi?.Dispose();
            MockServers.EmploymentCheckApi?.Dispose();

            OuterApi.Factory?.Dispose();
            OuterApi.Client?.Dispose();

            OuterApi.Factory = null;
            OuterApi.Client = null;

            MockServers.InnerApi = null;
            MockServers.AccountsApi = null;
            MockServers.CommitmentsV2InnerApi = null;
            MockServers.FinanceApi = null;
        }
    }
}
