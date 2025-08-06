using TechTalk.SpecFlow;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Bindings;

[Binding]
public class TestCleanUp
{
    [AfterTestRun()]
    public static void CleanUp()
    {
        NUnit.Framework.TestContext.WriteLine("Cleaning up...");

        MockServers.EarningsApi?.Dispose();
        MockServers.ApprenticeshipsApi?.Dispose();
        MockServers.CollectionCalendarApi?.Dispose();

        OuterApi.Factory?.Dispose();
        OuterApi.Client?.Dispose();

        OuterApi.Factory = null;
        OuterApi.Client = null;

        MockServers.EarningsApi = null;
        MockServers.ApprenticeshipsApi = null;
        MockServers.CollectionCalendarApi = null;
    }
}
