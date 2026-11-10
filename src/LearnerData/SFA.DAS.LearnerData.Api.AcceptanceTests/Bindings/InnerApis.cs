using SFA.DAS.LearnerData.Api.AcceptanceTests;
using System.Net;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Bindings;

[Binding]

public class InnerApis(TestContext context)
{
    public static HttpClient Client { get; set; }
    public static LocalWebApplicationFactory<Program> Factory { get; set; }

    public const decimal DefaultEnglishAndMathsValue = 1000m;

    [BeforeScenario(Order = 1)]
    public void Initialise()
    {
        NUnit.Framework.TestContext.WriteLine("Initialising inner apis...");

        StubMessageSession.SentMessages.Clear();
        StubMessageSession.PublishedMessages.Clear();

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

        if (context.SldDataApi == null)
        {
            context.SldDataApi = new MockApi();
        }

        // Setup default English and Maths value
        context.SldDataApi.MockServer
            .Given(Request.Create().WithPath("/api/v1.0/lars/get-em-value/*").UsingGet())
            .AtPriority(int.MaxValue)
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithBodyAsJson(DefaultEnglishAndMathsValue));

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
                {"SLDDataApiConfiguration:url", context?.SldDataApi?.BaseAddress + "/"},
                {"SLDDataApiConfiguration:TokenSettings:url", context?.SldDataApi?.BaseAddress + "/"},
                {"SLDDataApiConfiguration:TokenSettings:ShouldSkipForLocal", "true"},
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
