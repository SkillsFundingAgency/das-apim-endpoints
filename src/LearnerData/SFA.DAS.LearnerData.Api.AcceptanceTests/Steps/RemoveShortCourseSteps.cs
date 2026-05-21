using AutoFixture;
using FluentAssertions;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
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
    private const string EpisodeKey = "ShortCourseEpisodeKey";

    [When(@"the short course is removed")]
    public async Task WhenTheShortCourseIsRemoved()
    {
        ConfigureLearningApi();
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
        var removedEpisodeKey = scenarioContext.Get<Guid>(EpisodeKey);
        var requests = testContext.EarningsApi.MockServer.LogEntries;

        requests.Should().ContainSingle(
            r => r.RequestMessage.Url.Contains($"/{learningKey}/shortCourses/{removedEpisodeKey}") &&
                 r.RequestMessage.Method == "DELETE",
            "Expected a DELETE request to the Earnings domain for the short course");
    }

    private void ConfigureLearningApi()
    {
        var learningKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var removedEpisodeKey = Guid.NewGuid();
        scenarioContext.Set(removedEpisodeKey, EpisodeKey);

        var responseBody = new DeleteShortCourseResponse
        {
            LearningKey = learningKey,
            LearnerKey = learningKey,
            RemovedEpisodeKey = removedEpisodeKey,
            Learner = new LearningInnerShortCourseLearner
            {
                Uln = _fixture.Create<long>().ToString(),
                FirstName = _fixture.Create<string>(),
                LastName = _fixture.Create<string>(),
                DateOfBirth = _fixture.Create<DateTime>()
            },
            Episodes =
            [
                new LearningInnerShortCourseEpisode
                {
                    Ukprn = ukprn,
                    EmployerAccountId = 12,
                    CourseCode = "ZSC00001",
                    CourseType = "ShortCourse",
                    LearningType = "ApprenticeshipUnit",
                    StartDate = DateTime.UtcNow.AddMonths(-6),
                    AgeAtStart = 20,
                    PlannedEndDate = DateTime.UtcNow.AddMonths(6),
                    IsApproved = true,
                    Price = 1000m,
                    LearnerRef = "LearnerRef",
                    EmployerType = "Levy"
                }
            ]
        };

        testContext.ApprenticeshipsApi.MockServer
            .Given(
                Request.Create()
                    .WithPath($"/{ukprn}/shortCourses/{learningKey}")
                    .UsingDelete()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(responseBody)
            );
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
