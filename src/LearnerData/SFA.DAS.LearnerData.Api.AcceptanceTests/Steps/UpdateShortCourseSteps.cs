using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.Payments.EarningEvents.Messages.External;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
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

    public UpdateShortCourseSteps(TestContext testContext, ScenarioContext scenarioContext)
    {
        _testContext = testContext;
        _scenarioContext = scenarioContext;
    }

    [Given(@"there is a short course learning")]
    public void GivenThereIsAShortCourseLearning()
    {
        _scenarioContext.Set(Guid.NewGuid(), ShortCourseLearnerKey);
        _scenarioContext.Set(_fixture.Create<long>(), UkprnKey);
    }

    [Given(@"the (.*) of short course passed is different to the value in the learning domain")]
    public void GivenTheDetailsOfShortCoursePassedIsDifferentToTheValueInTheLearningDomain(ShortCourseUpdateChanges change)
    {
        List<ShortCourseUpdateChanges> changes;

        if (!_scenarioContext.TryGetValue(ShortCourseChangesKey, out changes))
        {
            changes = new List<ShortCourseUpdateChanges>();
        }

        changes.Add(change);

        _scenarioContext.Set(changes, ShortCourseChangesKey);
    }

    [When(@"the short course learning is updated")]
    public async Task WhenTheShortCourseLearningIsUpdated()
    {
        var learningKey = _scenarioContext.Get<Guid>(ShortCourseLearnerKey);
        var ukprn = _scenarioContext.Get<long>(UkprnKey);
        var requestBody = _fixture.Create<ShortCourseRequest>();

        ConfigureLearnerInnerApi(ukprn, learningKey, requestBody);
        ConfigureEarningsInnerApiToRespondeOkToEverything();
        await CallUpdateShortCourseLearningEndpoint(ukprn, learningKey, requestBody);
    }

    [Then(@"a on-programme update request is sent for short courses to the earnings domain")]
    public void ThenAOn_ProgrammeUpdateRequestIsSentForShortCoursesToTheEarningsDomain()
    {
        var learnerKey = _scenarioContext.Get<Guid>(ShortCourseLearnerKey);
        var requestUrl = $"/{learnerKey}/shortCourses/on-programme";
        var requests = _testContext.EarningsApi.MockServer.LogEntries;

        requests.Should().ContainSingle(request => request.RequestMessage.Url.Contains(requestUrl),
            $"Expected a request to {requestUrl} but found {requests.Count} requests instead.");
    }

    [Then(@"a short course earnings updated event is published for payments")]
    public void ThenAShortCourseEarningsUpdatedEventIsPublishedForPayments()
    {
        var learnerKey = _scenarioContext.Get<Guid>(ShortCourseLearnerKey);
        var calculateGrowthAndSkillsPayments = StubMessageSession.SentMessages
            .OfType<CalculateGrowthAndSkillsPayments>()
            .Where(e => e.Learner.LearnerKey == learnerKey)
            .ToList();

        calculateGrowthAndSkillsPayments.Should().NotBeEmpty("Expected a CalculateGrowthAndSkillsPayments command to be sent but none were found.");
        calculateGrowthAndSkillsPayments.Should().ContainSingle(e => e.Training.CourseType == Payments.EarningEvents.Messages.External.CourseType.ShortCourse,
            "Expected a CalculateGrowthAndSkillsPayments command for a ShortCourse to be sent but it was not found.");
    }

    private void ConfigureLearnerInnerApi(long ukprn, Guid learningKey, ShortCourseRequest shortCourseRequest)
    {
        var changes = _scenarioContext.Get<List<ShortCourseUpdateChanges>>(ShortCourseChangesKey);
        var onProgramme = shortCourseRequest.Delivery.OnProgramme.First();

        var response = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = learningKey,
            Changes = changes.Select(x => x.ToString()).ToArray(),
            CompletionDate = onProgramme.ActualEndDate,
            Learner = new UpdateShortCourseResultLearner
            {
                Uln = shortCourseRequest.Learner.Uln.ToString(),
                FirstName = shortCourseRequest.Learner.FirstName,
                LastName = shortCourseRequest.Learner.LastName,
                DateOfBirth = shortCourseRequest.Learner.Dob,
            },
            Episodes = [new UpdateShortCourseResultEpisode
            {
                Ukprn = ukprn,
                EmployerAccountId = 12,
                CourseCode = "ZSC00001",
                CourseType = "ShortCourse",
                LearningType = "ApprenticeshipUnit",
                StartDate = onProgramme.StartDate,
                AgeAtStart = 20,
                PlannedEndDate = onProgramme.ExpectedEndDate,
                WithdrawalDate = onProgramme.WithdrawalDate,
                IsApproved = true,
                Price = 1000m,
                LearnerRef = "LearnerRef",
                EmployerType = EmployerType.Levy.ToString()
            }]
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

    private async Task CallUpdateShortCourseLearningEndpoint(long ukprn, Guid learningKey, ShortCourseRequest requestBody)
    {

        var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), new MediaTypeHeaderValue("application/json"));
        var response = await _testContext.OuterApiClient.PutAsync($"/providers/{ukprn}/shortCourses/{learningKey}", httpContent);
        var contentString = await response.Content.ReadAsStringAsync();
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer Api call, but got {response.StatusCode}. Content: {contentString}");
    }
}
