using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using System.Net;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
public class CreateLearnerSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private const string UkprnKey = "CreateLearnerUkprnKey";
    private const string EnglishAndMathsKey = "CreateLearnerEnglishAndMathsKey";
    private const string OuterApiResponseKey = "CreateLearnerOuterApiResponse";

    private const int StandardCode = 91;
    private const string LearnAimRef = "60075880";
    private static readonly DateTime StartDate = new(2026, 8, 1);
    private static readonly DateTime ExpectedEndDate = new(2027, 7, 31);

    [Given(@"a provider is creating an apprenticeship learner")]
    public void GivenAProviderIsCreatingAnApprenticeshipLearner()
    {
        scenarioContext.Set(10005088L, UkprnKey);
    }

    [Given(@"the learner has the following English and maths items for the same course:")]
    public void GivenTheLearnerHasTheFollowingEnglishAndMathsItemsForTheSameCourse(Table table)
    {
        var items = table.Rows.Select(row => new MathsAndEnglish
        {
            Course = "Maths",
            LearnAimRef = LearnAimRef,
            StartDate = DateTime.Parse(row["StartDate"]),
            EndDate = DateTime.Parse(row["EndDate"]),
            PauseDate = ParseNullableDate(row["PauseDate"]),
            CompletionDate = ParseNullableDate(row["CompletionDate"]),
            LearningSupport = []
        }).ToList();

        scenarioContext.Set(items, EnglishAndMathsKey);
    }

    [When(@"the apprenticeship learner is created")]
    public async Task WhenTheApprenticeshipLearnerIsCreated()
    {
        ConfigureCoursesApi();
        ConfigureLearningInnerApi();
        ConfigureEarningsInnerApiToRespondOkToEverything();
        await CallCreateLearnerEndpoint();
    }

    [Then(@"the earnings domain receives the following English and maths periods in learning:")]
    public void ThenTheEarningsDomainReceivesTheFollowingEnglishAndMathsPeriodsInLearning(Table table)
    {
        var response = scenarioContext.Get<HttpResponseMessage>(OuterApiResponseKey);
        response.StatusCode.Should().Be(HttpStatusCode.Accepted, "the outer API should accept the create request");

        var entry = testContext.EarningsApi.MockServer.LogEntries
            .Single(x => x.RequestMessage.Method == "POST" && x.RequestMessage.Path == "/learning");

        var body = JsonConvert.DeserializeObject<CreateUnapprovedApprenticeshipLearningRequest>(entry.RequestMessage.Body!);

        var actual = body!.EnglishAndMaths.Single().PeriodsInLearning
            .Select(x => new { x.StartDate, x.EndDate, x.OriginalExpectedEndDate })
            .ToList();

        var expected = table.Rows
            .Select(row => new
            {
                StartDate = DateTime.Parse(row["StartDate"]),
                EndDate = ParseNullableDate(row["EndDate"]),
                OriginalExpectedEndDate = DateTime.Parse(row["OriginalExpectedEndDate"])
            })
            .ToList();

        actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    private void ConfigureCoursesApi()
    {
        var response = new StandardDetailResponse
        {
            ApprenticeshipType = "Apprenticeship",
            ApprenticeshipFunding =
            [
                new ApprenticeshipFunding
                {
                    EffectiveFrom = new DateTime(2020, 1, 1),
                    EffectiveTo = null,
                    MaxEmployerLevyCap = 15000
                }
            ]
        };

        testContext.CoursesApi.MockServer
            .Given(Request.Create().WithPath("/api/courses/standards/*").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithBodyAsJson(response));
    }

    private void ConfigureLearningInnerApi()
    {
        var ukprn = scenarioContext.Get<long>(UkprnKey);

        var response = new CreateDraftLearnerApiPutResponse
        {
            LearningKey = Guid.NewGuid(),
            LearningEpisodeKey = Guid.NewGuid(),
            Prices =
            [
                new BaseLearnerApiPutResponse.EpisodePrice
                {
                    Key = Guid.NewGuid(),
                    StartDate = StartDate,
                    EndDate = ExpectedEndDate,
                    TrainingPrice = 12000,
                    EndPointAssessmentPrice = 3000,
                    TotalPrice = 15000
                }
            ]
        };

        testContext.ApprenticeshipsApi.MockServer
            .Given(Request.Create().WithPath($"/{ukprn}/apprenticeships").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithBodyAsJson(response));
    }

    private void ConfigureEarningsInnerApiToRespondOkToEverything()
    {
        testContext.EarningsApi.MockServer
            .Given(Request.Create().UsingAnyMethod().WithPath(new WildcardMatcher("*")))
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));
    }

    private async Task CallCreateLearnerEndpoint()
    {
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var englishAndMaths = scenarioContext.Get<List<MathsAndEnglish>>(EnglishAndMathsKey);

        var request = new CreateLearnerRequest
        {
            ConsumerReference = "CR-TEST",
            Learner = new CreateLearnerRequest.LearnerDetails
            {
                Uln = 7353272693,
                LearnerRef = "LR-TEST",
                FirstName = "Test",
                LastName = "Learner",
                Dob = new DateTime(2000, 1, 1)
            },
            Delivery = new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme =
                [
                    new CreateLearnerRequest.OnProgrammeDetails
                    {
                        AimSequenceNumber = 1,
                        StandardCode = StandardCode,
                        LearnAimRef = "ZPROG001",
                        StartDate = StartDate,
                        ExpectedEndDate = ExpectedEndDate,
                        PercentageOfTrainingLeft = 80,
                        IsFlexiJob = false,
                        Costs = [new CostDetails { TrainingPrice = 12000, EpaoPrice = 3000, FromDate = StartDate }],
                        LearningSupport = [],
                        Care = new SFA.DAS.LearnerData.Requests.Care()
                    }
                ],
                EnglishAndMaths = englishAndMaths
            }
        };

        var httpContent = new StringContent(JsonConvert.SerializeObject(request), new MediaTypeHeaderValue("application/json"));
        var response = await testContext.OuterApiClient.PostAsync($"/providers/{ukprn}/apprenticeships", httpContent);
        scenarioContext.Set(response, OuterApiResponseKey);
    }

    private static DateTime? ParseNullableDate(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : DateTime.Parse(value);
    }
}
