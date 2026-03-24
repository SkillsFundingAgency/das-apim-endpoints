using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
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

    [Given(@"the learning domain will return no content for the short course creation")]
    public void GivenTheLearningDomainWillReturnNoContentForTheShortCourseCreation()
    {
        _scenarioContext.Set(true, LearningReturnsNoContentKey);
    }

    [When(@"a draft short course is created for the provider")]
    public async Task WhenADraftShortCourseIsCreatedForTheProvider()
    {
        ConfigureLearningInnerApi();
        ConfigureEarningsInnerApiToRespondOkToEverything();
        await CallCreateDraftShortCourseEndpoint();
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
}
