using TechTalk.SpecFlow;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Bindings;

[Binding]

public class InnerApis(TestContext context)
{
    public static HttpClient Client { get; set; }
    public static LocalWebApplicationFactory<Program> Factory { get; set; }

    [BeforeScenario(Order = 1)]
    public void Initialise()
    {
        NUnit.Framework.TestContext.WriteLine("Initialising inner apis...");

        if (context.EarningsApi == null)
        {
            context.EarningsApi = new MockApi();
        }

        if (context.ApprenticeshipsApi == null)
        {
            context.ApprenticeshipsApi = new MockApi();
        }

        if (context.CollectionCalendarApi == null)
        {
            context.CollectionCalendarApi = new MockApi();
        }

        if (context.CoursesApi == null)
        {
            context.CoursesApi = new MockApi();
        }

        NUnit.Framework.TestContext.WriteLine("Initialising outer api...");
        if (Client == null)
        {
            var config = new Dictionary<string, string>
            {
                {"Environment", "LOCAL_ACCEPTANCE_TESTS"},
                {"EarningsApiConfiguration:url", context?.EarningsApi?.BaseAddress + "/"},
                {"ApprenticeshipsApiConfiguration:url", context?.ApprenticeshipsApi?.BaseAddress + "/"},
                {"CollectionCalendarApiConfiguration:url", context?.CollectionCalendarApi?.BaseAddress + "/"},
                {"CoursesApiConfiguration:url", context?.CoursesApi?.BaseAddress + "/"},
                {"AzureAD:tenant", ""},
                {"AzureAD:identifier", ""},
                {"UseInMemoryCache", "true"}
            };


            Factory = new LocalWebApplicationFactory<Program>(config);
            Client = Factory.CreateClient();
        }

        context.OuterApiClient = Client;
    }
}
