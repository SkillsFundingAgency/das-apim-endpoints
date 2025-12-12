using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using System.Net;
using TechTalk.SpecFlow;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
internal class RemoveLearnerSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private const string LearnerKey = "LearnerKey";
    private const string UkprnKey = "UkprnKey";

    [When(@"the learner is removed")]
    public async Task WhenTheLearnerIsRemoved()
    {
        ConfigureRemoveLearningApi();
        ConfigureWithdrawLearnerEarningsApi();
        await CallRemoveLearnerEndpoint();
    }

    [Then(@"a remove learning request is sent to the learning domain")]
    public void ThenARemoveLearningRequestIsSentToTheLearningDomain()
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var requests = testContext.ApprenticeshipsApi.MockServer.LogEntries;

        requests.Should().ContainSingle(request => request.RequestMessage.Url.Contains($"/{ukprn}/{learnerKey}") &&
                                                   request.RequestMessage.Method == "DELETE",
            "Expected a DELETE request to the Learning domain for the learner");
    }

    [Then(@"a delete learner request is sent to the earnings domain")]
    public void ThenADeleteLearnerRequestIsSentToTheEarningsDomain()
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);
        var requests = testContext.EarningsApi.MockServer.LogEntries;

        requests.Should().ContainSingle(request => request.RequestMessage.Url.Contains($"/{learnerKey}") && request.RequestMessage.Method == "DELETE",
            "Expected a delete request to the Earnings domain for the learner");
    }

    private void ConfigureRemoveLearningApi()
    {
        var learningKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);

        var responseBody = new RemoveLearnerResponse
        {
            LastDayOfLearning = DateTime.UtcNow
        };

        testContext.ApprenticeshipsApi.MockServer
            .Given(
                Request.Create()
                    .WithPath($"/{ukprn}/{learningKey}")
                    .UsingDelete()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(responseBody)
            );
    }


    private void ConfigureWithdrawLearnerEarningsApi()
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);

        testContext.EarningsApi.MockServer
            .Given(
                Request.Create()
                    .WithPath($"/learning/{learnerKey}")
                    .UsingDelete()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.NoContent)
            );
    }

    private async Task CallRemoveLearnerEndpoint()
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var response = await testContext.OuterApiClient.DeleteAsync($"/providers/{ukprn}/learning/{learnerKey}");
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer API call, but got {response.StatusCode}");
    }
}
