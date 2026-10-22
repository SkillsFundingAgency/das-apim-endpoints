using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Requests;
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
internal class CreateLearnerSteps(TestContext testContext, ScenarioContext scenarioContext, FurtherApprovalSteps furtherApprovalSteps)
{
    private readonly Fixture _fixture = new Fixture();
    private const string UkprnKey = "CreateLearnerUkprnKey";
    private const string ChangesKey = "CreateLearnerChangesKey";

    [Given(@"there is an approved apprenticeship that was removed")]
    public void GivenThereIsAnApprovedApprenticeshipThatWasRemoved()
    {
        scenarioContext.Set(_fixture.Create<long>(), UkprnKey);
        scenarioContext.Set(new List<BaseLearnerApiPutResponse.LearningUpdateChanges> { BaseLearnerApiPutResponse.LearningUpdateChanges.Reinstated }, ChangesKey);
    }

    [Given(@"the reinstating submission changes the start date")]
    public void GivenTheReinstatingSubmissionChangesTheStartDate()
    {
        scenarioContext.Get<List<BaseLearnerApiPutResponse.LearningUpdateChanges>>(ChangesKey).AddRange(
        [
            BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate,
            BaseLearnerApiPutResponse.LearningUpdateChanges.Prices
        ]);
    }

    [When(@"the learner is reinstated")]
    public async Task WhenTheLearnerIsReinstated()
    {
        ConfigureCoursesApi();
        ConfigureLearnerInnerApi();
        ConfigureEarningsInnerApiToRespondOkToEverything();
        await CallCreateLearnerEndpoint();
    }

    private void ConfigureCoursesApi()
    {
        testContext.CoursesApi.MockServer
            .Given(Request.Create().WithPath("/api/courses/standards/*"))
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithBodyAsJson(new StandardDetailResponse { ApprenticeshipType = "Apprenticeship" }));
    }

    private void ConfigureLearnerInnerApi()
    {
        var ukprn = scenarioContext.Get<long>(UkprnKey);

        var response = new CreateDraftLearnerApiPutResponse
        {
            LearningKey = Guid.NewGuid(),
            LearningEpisodeKey = Guid.NewGuid(),
            IsApproved = true
        };
        response.Changes.AddRange(scenarioContext.Get<List<BaseLearnerApiPutResponse.LearningUpdateChanges>>(ChangesKey));

        furtherApprovalSteps.ConfigureApprovalFlowIfNeeded(response);

        testContext.ApprenticeshipsApi.MockServer
            .Given(Request.Create()
                .WithPath($"/{ukprn}/apprenticeships")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                // System.Text.Json, not WithBodyAsJson (Newtonsoft): the response holds JsonNode values in its change details
                .WithBody(System.Text.Json.JsonSerializer.Serialize(response)));

        scenarioContext.Set<BaseLearnerApiPutResponse>(response);
    }

    private void ConfigureEarningsInnerApiToRespondOkToEverything()
    {
        testContext.EarningsApi.MockServer
            .Given(Request.Create()
                .UsingAnyMethod()
                .WithPath(new WildcardMatcher("*")))
            .RespondWith(Response.Create().WithStatusCode(200));
    }

    private async Task CallCreateLearnerEndpoint()
    {
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var requestBody = _fixture.Create<CreateLearnerRequest>();

        var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), new MediaTypeHeaderValue("application/json"));
        var response = await testContext.OuterApiClient.PostAsync($"/providers/{ukprn}/learners", httpContent);
        var contentString = await response.Content.ReadAsStringAsync();
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer Api call, but got {response.StatusCode}. Content: {contentString}");
    }
}
