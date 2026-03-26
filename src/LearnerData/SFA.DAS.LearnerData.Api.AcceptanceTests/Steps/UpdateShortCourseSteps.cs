using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
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

        ConfigureLearnerInnerApi(learningKey, requestBody);
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

    private void ConfigureLearnerInnerApi(Guid learningKey, ShortCourseRequest shortCourseRequest)
    {
        var changes = _scenarioContext.Get<List<ShortCourseUpdateChanges>>(ShortCourseChangesKey);
        var onProgramme = shortCourseRequest.Delivery.OnProgramme.First();

        var response = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = learningKey,
            Changes = changes.Select(x => x.ToString()).ToArray(),
            Price = 1000m,
            Uln = shortCourseRequest.Learner.Uln,
            LearnerRef = "LearnerRef",
            LearningType = "2",
            TrainingCode = "ZSC00001",
            AgeAtStart = 20,
            StartDate = onProgramme.StartDate,
            PlannedEndDate = onProgramme.ExpectedEndDate,
            WithdrawalDate = onProgramme.WithdrawalDate,
            CompletionDate = onProgramme.ActualEndDate,
            EmployerAccountId = 12
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
        var response = new UpdateShortCourseEarningPutResponse();
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
