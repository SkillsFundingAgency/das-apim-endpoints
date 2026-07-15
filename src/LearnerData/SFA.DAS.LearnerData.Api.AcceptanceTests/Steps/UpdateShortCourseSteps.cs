using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.Payments.EarningEvents.Messages.External;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using System.Net;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
public class UpdateShortCourseSteps
{
    private readonly Fixture _fixture = new Fixture();
    private readonly TestContext _testContext;
    private readonly ScenarioContext _scenarioContext;
    private const string ShortCourseLearnerKey = "ShortCourseLearnerKey";
    private const string UkprnKey = "ShortCourseUkprnKey";
    private const string ShortCourseChangesKey = "ShortCourseChanges";
    private const string UpdatedEpisodeKeyKey = "UpdatedEpisodeKey";
    private const string IsApprovedKey = "ShortCourseIsApproved";
    private const string ShortCourseRequestKey = "ShortCourseRequest";
    private const string PersistedStartDateOverrideKey = "PersistedStartDateOverride";

    public UpdateShortCourseSteps(TestContext testContext, ScenarioContext scenarioContext)
    {
        _testContext = testContext;
        _scenarioContext = scenarioContext;
    }

    [Given(@"there is a short course learning")]
    public void GivenThereIsAShortCourseLearning()
    {
        SetUpShortCourseLearning(isApproved: true);
    }

    [Given(@"there is an (approved|unapproved) short course learning")]
    public void GivenThereIsAnApprovalStateShortCourseLearning(string approvalState)
    {
        SetUpShortCourseLearning(isApproved: approvalState == "approved");
    }

    private void SetUpShortCourseLearning(bool isApproved)
    {
        _scenarioContext.Set(Guid.NewGuid(), ShortCourseLearnerKey);
        _scenarioContext.Set(_fixture.Create<long>(), UkprnKey);
        _scenarioContext.Set(isApproved, IsApprovedKey);
    }

    [Given(@"SLD inform us that the (.*) has changed")]
    public void GivenSLDInformUsThatTheChangeHasChanged(ShortCourseUpdateChanges change)
    {
        List<ShortCourseUpdateChanges> changes;

        if (!_scenarioContext.TryGetValue(ShortCourseChangesKey, out changes))
        {
            changes = new List<ShortCourseUpdateChanges>();
        }

        changes.Add(change);

        _scenarioContext.Set(changes, ShortCourseChangesKey);
    }

    [Given(@"learning ignores the change and persists a different StartDate")]
    public void GivenLearningIgnoresTheChangeAndPersistsADifferentStartDate()
    {
        _scenarioContext.Set(new DateTime(2020, 1, 1), PersistedStartDateOverrideKey);
    }

    [When(@"the short course learning is updated")]
    public async Task WhenTheShortCourseLearningIsUpdated()
    {
        var learningKey = _scenarioContext.Get<Guid>(ShortCourseLearnerKey);
        var ukprn = _scenarioContext.Get<long>(UkprnKey);
        var requestBody = _fixture.Create<ShortCourseRequest>();
        _scenarioContext.Set(requestBody, ShortCourseRequestKey);

        ConfigureLearnerInnerApi(ukprn, learningKey, requestBody);
        ConfigureEarningsInnerApiToRespondeOkToEverything();
        ConfigureCoursesApi();
        await CallUpdateShortCourseLearningEndpoint(ukprn, learningKey, requestBody);
    }

    [Then(@"a on-programme update request is sent for short courses to the earnings domain")]
    public void ThenAOn_ProgrammeUpdateRequestIsSentForShortCoursesToTheEarningsDomain()
    {
        var learnerKey = _scenarioContext.Get<Guid>(ShortCourseLearnerKey);
        var episodeKey = _scenarioContext.Get<Guid>(UpdatedEpisodeKeyKey);
        var requestUrl = $"/{learnerKey}/shortCourses/{episodeKey}/on-programme";
        var requests = _testContext.EarningsApi.MockServer.LogEntries;

        requests.Should().ContainSingle(request => request.RequestMessage.Url.Contains(requestUrl),
            $"Expected a request to {requestUrl} but found {requests.Count} requests instead.");
    }

    [Then(@"the on-programme update sent to earnings has the learning-persisted StartDate, not the SLD payload's StartDate")]
    public void ThenTheOnProgrammeUpdateSentToEarningsHasTheLearningPersistedStartDate()
    {
        var persistedStartDate = _scenarioContext.Get<DateTime>(PersistedStartDateOverrideKey);
        var sldStartDate = _scenarioContext.Get<ShortCourseRequest>(ShortCourseRequestKey).Delivery.OnProgramme.First().StartDate;
        persistedStartDate.Should().NotBe(sldStartDate, "the test setup should use different dates or this assertion proves nothing");

        var learnerKey = _scenarioContext.Get<Guid>(ShortCourseLearnerKey);
        var episodeKey = _scenarioContext.Get<Guid>(UpdatedEpisodeKeyKey);
        var requestUrl = $"/{learnerKey}/shortCourses/{episodeKey}/on-programme";

        var entry = _testContext.EarningsApi.MockServer.LogEntries
            .Single(request => request.RequestMessage.Url.Contains(requestUrl));

        var body = JsonConvert.DeserializeObject<SFA.DAS.LearnerData.Requests.LearningInner.UpdateShortCourseOnProgrammeRequestBody>(entry.RequestMessage.Body);
        body.StartDate.Should().Be(persistedStartDate);
    }

    private void ConfigureLearnerInnerApi(long ukprn, Guid learningKey, ShortCourseRequest shortCourseRequest)
    {
        var changes = _scenarioContext.Get<List<ShortCourseUpdateChanges>>(ShortCourseChangesKey);
        var isApproved = _scenarioContext.Get<bool>(IsApprovedKey);
        var onProgramme = shortCourseRequest.Delivery.OnProgramme.First();

        var updatedEpisodeKey = Guid.NewGuid();
        _scenarioContext.Set(updatedEpisodeKey, UpdatedEpisodeKeyKey);

        var response = new UpdateShortCourseLearningResponse
        {
            Results =
            [
                new UpdateShortCourseLearningPutResponse
                {
                    UpdatedEpisodeKey = updatedEpisodeKey,
                    LearningKey = learningKey,
                    LearnerKey = learningKey,
                    Changes = changes.Select(x => x.ToString()).ToArray(),
                    Learner = new LearningInnerShortCourseLearner
                    {
                        Uln = shortCourseRequest.Learner.Uln.ToString(),
                        FirstName = shortCourseRequest.Learner.FirstName,
                        LastName = shortCourseRequest.Learner.LastName,
                        DateOfBirth = shortCourseRequest.Learner.Dob,
                    },
                    Episodes = [new LearningInnerShortCourseEpisode
                    {
                        EpisodeKey = updatedEpisodeKey,
                        Ukprn = ukprn,
                        EmployerAccountId = 12,
                        CourseCode = "ZSC00001",
                        CourseType = "ShortCourse",
                        LearningType = "ApprenticeshipUnit",
                        StartDate = _scenarioContext.TryGetValue(PersistedStartDateOverrideKey, out DateTime persistedStartDate)
                            ? persistedStartDate
                            : onProgramme.StartDate,
                        AgeAtStart = 20,
                        PlannedEndDate = onProgramme.ExpectedEndDate,
                        WithdrawalDate = onProgramme.WithdrawalDate,
                        CompletionDate = onProgramme.CompletionDate,
                        IsApproved = isApproved,
                        Price = 1000m,
                        LearnerRef = "LearnerRef",
                        EmployerType = EmployerType.Levy.ToString()
                    }]
                }
            ]
        };

        _testContext.ApprenticeshipsApi.MockServer
        .Given(
            Request
            .Create()
            .WithPath($"/shortCourses/{learningKey}")
            .UsingPut())
        .RespondWith(
            Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithBodyAsJson(response)
        );
    }

    private void ConfigureEarningsInnerApiToRespondeOkToEverything()
    {

        var response = new UpdateShortCourseEarningPutResponse
        {
            EarningProfileVersion = Guid.NewGuid(),
            Instalments = [new ShortCourseInstalment
            {
                Amount = 100m,
                CollectionPeriod = 1,
                CollectionYear = 2023,
                Type = "ThirtyPercentLearningComplete",
                IsPayable = true
            },
            new ShortCourseInstalment
            {
                Amount = 100m,
                CollectionPeriod = 1,
                CollectionYear = 2023,
                Type = "LearningComplete",
                IsPayable = true
            }]
        };

        _testContext.EarningsApi.MockServer
            .Given(
                Request.Create()
                    .UsingAnyMethod()
                    .WithPath(new WildcardMatcher("*")) // matches everything
            )
            .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(response)
            );
    }

    private void ConfigureCoursesApi()
    {
        var response = new CourseLookupDetailResponse
        {
            LarsCode = _fixture.Create<string>(),
            Title = _fixture.Create<string>(),
            LearningType = "ApprenticeshipUnit",
            CourseType = _fixture.Create<string>(),
            ApprenticeshipFunding =
            [
                new ApprenticeshipFunding
                {
                    MaxEmployerLevyCap = 6000,
                    EffectiveFrom = new DateTime(2020, 1, 1),
                    EffectiveTo = null
                }
            ]
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

    private async Task CallUpdateShortCourseLearningEndpoint(long ukprn, Guid learningKey, ShortCourseRequest requestBody)
    {

        var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), new MediaTypeHeaderValue("application/json"));
        var response = await _testContext.OuterApiClient.PutAsync($"/providers/{ukprn}/shortCourses/{learningKey}", httpContent);
        var contentString = await response.Content.ReadAsStringAsync();
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer Api call, but got {response.StatusCode}. Content: {contentString}");
    }
}
