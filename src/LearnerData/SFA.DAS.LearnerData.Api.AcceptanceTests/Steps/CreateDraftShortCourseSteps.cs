using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.LearnerData.Responses.LearningInner;
using System.Net;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
public class CreateDraftShortCourseSteps
{
    private readonly Fixture _fixture = new Fixture();
    private readonly TestContext _testContext;
    private readonly ScenarioContext _scenarioContext;
    private const string UkprnKey = "CreateDraftShortCourseUkprnKey";
    private const string LearningReturnsNoContentKey = "LearningReturnsNoContent";
    private const string FundingBandsKey = "FundingBands";
    private const string CoursesApiErrorKey = "CoursesApiError";

    public CreateDraftShortCourseSteps(TestContext testContext, ScenarioContext scenarioContext)
    {
        _testContext = testContext;
        _scenarioContext = scenarioContext;
    }

    [Given(@"there is a provider")]
    public void GivenThereIsAProvider()
    {
        _scenarioContext.Set(_fixture.Create<long>(), UkprnKey);
    }

    [Given(@"the courses api has the following funding bands")]
    public void GivenTheCoursesApiHasTheFollowingFundingBands(Table table)
    {
        var bands = table.Rows.Select(row => new ApprenticeshipFunding
        {
            EffectiveFrom = DateTime.Parse(row["From"]),
            EffectiveTo = string.IsNullOrEmpty(row["To"]) ? (DateTime?)null : DateTime.Parse(row["To"]),
            MaxEmployerLevyCap = int.Parse(row["Price"])
        }).ToList();
        _scenarioContext.Set(bands, FundingBandsKey);
    }

    [Given(@"the courses api will return an error")]
    public void GivenTheCoursesApiWillReturnAnError()
    {
        _scenarioContext.Set(true, CoursesApiErrorKey);
    }

    [Given(@"the courses api returns a course with no funding bands")]
    public void GivenTheCoursesApiReturnsACourseWithNoFundingBands()
    {
        _scenarioContext.Set(new List<ApprenticeshipFunding>(), FundingBandsKey);
    }

    [Given(@"the learning domain will return no content for the short course creation")]
    public void GivenTheLearningDomainWillReturnNoContentForTheShortCourseCreation()
    {
        _scenarioContext.Set(true, LearningReturnsNoContentKey);
    }

    [When(@"a draft short course is created for the provider")]
    public async Task WhenADraftShortCourseIsCreatedForTheProvider()
    {
        ConfigureCoursesApi();
        ConfigureLearningInnerApi();
        ConfigureEarningsInnerApiToRespondOkToEverything();
        await CallCreateDraftShortCourseEndpoint();
    }

    [When(@"a draft short course is created for the provider with start date (.*) and learning type (.*)")]
    public async Task WhenADraftShortCourseIsCreatedForTheProviderWithStartDateAndLearningType(DateTime startDate, string learningType)
    {
        ConfigureCoursesApi(learningType);
        ConfigureLearningInnerApi();
        ConfigureEarningsInnerApiToRespondOkToEverything();
        await CallCreateDraftShortCourseEndpoint(startDate);
    }

    [Then(@"a short course creation request is sent to the earnings domain")]
    public void ThenAShortCourseCreationRequestIsSentToTheEarningsDomain()
    {
        var requests = _testContext.EarningsApi.MockServer.LogEntries;
        requests.Should().ContainSingle(r => r.RequestMessage.Url.Contains("shortCourses"),
            $"Expected a POST request to the earnings shortCourses endpoint but found {requests.Count} requests instead.");
    }

    [Then(@"the earnings domain is not called")]
    public void ThenTheEarningsDomainIsNotCalled()
    {
        var requests = _testContext.EarningsApi.MockServer.LogEntries;
        requests.Should().BeEmpty("Expected no requests to the earnings domain when learning returns NoContent.");
    }

    [Then(@"the earnings domain receives a price of (.*) and learning type (.*)")]
    public void ThenTheEarningsDomainReceivesAPriceOfAndLearningType(int expectedPrice, LearningType expectedLearningType)
    {
        var entry = _testContext.EarningsApi.MockServer.LogEntries
            .Single(r => r.RequestMessage.Url.Contains("shortCourses"));

        var body = JsonConvert.DeserializeObject<CreateUnapprovedShortCourseLearningRequest>(entry.RequestMessage.Body);
        body.OnProgramme.TotalPrice.Should().Be(expectedPrice);
        body.OnProgramme.LearningType.Should().Be(expectedLearningType);
    }

    private void ConfigureCoursesApi(string learningType = "ApprenticeshipUnit")
    {
        if (_scenarioContext.ContainsKey(CoursesApiErrorKey))
        {
            _testContext.CoursesApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/api/courses/lookup/*")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.InternalServerError)
                );
            return;
        }

        var fundingBands = _scenarioContext.ContainsKey(FundingBandsKey)
            ? _scenarioContext.Get<List<ApprenticeshipFunding>>(FundingBandsKey)
            : new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding
                {
                    MaxEmployerLevyCap = 6000,
                    EffectiveFrom = new DateTime(2020, 1, 1),
                    EffectiveTo = null
                }
            };

        var response = new CourseLookupDetailResponse
        {
            LarsCode = _fixture.Create<string>(),
            Title = _fixture.Create<string>(),
            LearningType = learningType,
            CourseType = _fixture.Create<string>(),
            ApprenticeshipFunding = fundingBands
        };

        _testContext.CoursesApi.MockServer
            .Given(
                Request.Create()
                    .WithPath("/api/courses/lookup/*")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(response)
            );
    }

    private void ConfigureLearningInnerApi()
    {
        var returnsNoContent = _scenarioContext.ContainsKey(LearningReturnsNoContentKey) &&
                               _scenarioContext.Get<bool>(LearningReturnsNoContentKey);

        if (returnsNoContent)
        {
            _testContext.ApprenticeshipsApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/shortCourses")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.NoContent)
                );
        }
        else
        {
            var learningResponse = new CreateShortCoursePostResponse
            {
                LearningKey = Guid.NewGuid(),
                EpisodeKey = Guid.NewGuid()
            };

            _testContext.ApprenticeshipsApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/shortCourses")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.Created)
                        .WithBodyAsJson(learningResponse)
                );
        }
    }

    private void ConfigureEarningsInnerApiToRespondOkToEverything()
    {
        _testContext.EarningsApi.MockServer
            .Given(
                Request.Create()
                    .UsingAnyMethod()
                    .WithPath(new WildcardMatcher("*"))
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
            );
    }

    private async Task CallCreateDraftShortCourseEndpoint()
    {
        var ukprn = _scenarioContext.Get<long>(UkprnKey);
        var requestBody = _fixture.Create<ShortCourseRequest>();
        var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), new MediaTypeHeaderValue("application/json"));
        var response = await _testContext.OuterApiClient.PostAsync($"/providers/{ukprn}/shortCourses", httpContent);
        var contentString = await response.Content.ReadAsStringAsync();
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer API but got {response.StatusCode}. Content: {contentString}");
    }

    private async Task CallCreateDraftShortCourseEndpoint(DateTime startDate)
    {
        var ukprn = _scenarioContext.Get<long>(UkprnKey);
        var requestBody = _fixture.Build<ShortCourseRequest>()
            .With(x => x.Delivery, new ShortCourseDelivery
            {
                OnProgramme =
                [
                    _fixture.Build<ShortCourseOnProgramme>()
                        .With(x => x.StartDate, startDate)
                        .With(x => x.ExpectedEndDate, startDate.AddMonths(12))
                        .With(x => x.CompletionDate, (DateTime?)null)
                        .With(x => x.WithdrawalDate, (DateTime?)null)
                        .With(x => x.Milestones, Array.Empty<Requests.Milestone>())
                        .Create()
                ]
            })
            .Create();
        var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), new MediaTypeHeaderValue("application/json"));
        var response = await _testContext.OuterApiClient.PostAsync($"/providers/{ukprn}/shortCourses", httpContent);
        var contentString = await response.Content.ReadAsStringAsync();
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer API but got {response.StatusCode}. Content: {contentString}");
    }
}
