using AutoFixture;
using FluentAssertions;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using System.Net;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
internal class RemoveShortCourseSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private readonly Fixture _fixture = new Fixture();
    private const string LearnerKey = "ShortCourseLearnerKey";
    private const string UkprnKey = "ShortCourseUkprnKey";
    private const string LearningNoContentKey = "ShortCourseRemovalLearningNoContent";

    [Given(@"the learning domain will return no content for the short course removal")]
    public void GivenTheLearningDomainWillReturnNoContentForTheShortCourseRemoval()
    {
        scenarioContext.Set(true, LearningNoContentKey);
    }

    [When(@"the short course is removed")]
    public async Task WhenTheShortCourseIsRemoved()
    {
        var noContent = scenarioContext.ContainsKey(LearningNoContentKey) && scenarioContext.Get<bool>(LearningNoContentKey);
        ConfigureLearningApi(noContent);
        if (!noContent)
            ConfigureEarningsApi();
        await CallRemoveShortCourseEndpoint();
    }

    [Then(@"a remove short course request is sent to the learning domain")]
    public void ThenARemoveShortCourseRequestIsSentToTheLearningDomain()
    {
        var learningKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var requests = testContext.ApprenticeshipsApi.MockServer.LogEntries;

        requests.Should().ContainSingle(
            r => r.RequestMessage.Url.Contains($"/{ukprn}/shortCourses/{learningKey}") &&
                 r.RequestMessage.Method == "DELETE",
            "Expected a DELETE request to the Learning domain for the short course");
    }

    [Then(@"a remove short course request is sent to the earnings domain")]
    public void ThenARemoveShortCourseRequestIsSentToTheEarningsDomain()
    {
        var learningKey = scenarioContext.Get<Guid>(LearnerKey);
        var requests = testContext.EarningsApi.MockServer.LogEntries;

        requests.Should().ContainSingle(
            r => r.RequestMessage.Url.Contains($"/{learningKey}/shortCourses/") &&
                 r.RequestMessage.Method == "DELETE",
            "Expected a DELETE request to the Earnings domain for the short course");
    }

    [Then(@"the earnings domain is not called for short course removal")]
    public void ThenTheEarningsDomainIsNotCalledForShortCourseRemoval()
    {
        var requests = testContext.EarningsApi.MockServer.LogEntries;
        requests.Should().BeEmpty("Expected no requests to the Earnings domain, but found some.");
    }

    private void ConfigureLearningApi(bool returnNoContent)
    {
        var learningKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);

        var builder = Response.Create().WithStatusCode(returnNoContent ? HttpStatusCode.NoContent : HttpStatusCode.OK);

        if (!returnNoContent)
        {
            var responseBody = new DeleteShortCourseResponse
            {
                LearningKey = learningKey,
                LearnerKey = learningKey,
                RemovedEpisodeKey = _fixture.Create<Guid>(),
                Learner = _fixture.Create<LearningInnerShortCourseLearner>(),
                Episodes = []
            };
            builder = builder.WithHeader("Content-Type", "application/json").WithBodyAsJson(responseBody);
        }

        testContext.ApprenticeshipsApi.MockServer
            .Given(
                Request.Create()
                    .WithPath($"/{ukprn}/shortCourses/{learningKey}")
                    .UsingDelete()
            )
            .RespondWith(builder);
    }

    private void ConfigureEarningsApi()
    {
        var learningKey = scenarioContext.Get<Guid>(LearnerKey);

        var responseBody = new DeleteShortCourseEarningsResponse
        {
            EarningProfileVersion = _fixture.Create<Guid>(),
            Instalments = []
        };

        testContext.EarningsApi.MockServer
            .Given(
                Request.Create()
                    .WithPath(new WireMock.Matchers.WildcardMatcher($"/{learningKey}/shortCourses/*"))
                    .UsingDelete()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(responseBody)
            );
    }

    private async Task CallRemoveShortCourseEndpoint()
    {
        var learningKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var response = await testContext.OuterApiClient.DeleteAsync($"/providers/{ukprn}/shortCourses/{learningKey}");
        response.IsSuccessStatusCode.Should().BeTrue(
            $"Expected successful response from outer API call, but got {response.StatusCode}");
    }
}
