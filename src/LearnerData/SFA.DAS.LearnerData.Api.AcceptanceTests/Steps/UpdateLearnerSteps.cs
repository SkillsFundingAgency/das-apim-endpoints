using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using System.Net;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
internal class UpdateLearnerSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private readonly Fixture _fixture = new Fixture();
    private const string ChangesKey = "Changes";
    private const string LearnerKey = "LearnerKey";
    private const string UkprnKey = "UkprnKey";
    private const string FundingBandMaximumKey = "FundingBandMaximumKey";
    private const string SldLearnerDataKey = "SldLearnerDataKey";

    [Given(@"there is a learner")]
    public void GivenThereIsALearner()
    {
        scenarioContext.Set(Guid.NewGuid(), LearnerKey);
        scenarioContext.Set(_fixture.Create<long>(), UkprnKey);
    }

    [Given(@"the (.*) passed is different to the value in the learners domain")]
    public void GivenTheCompletionDatePassedIsDifferentToTheValueInTheLearnersDomain(UpdateLearnerApiPutResponse.LearningUpdateChanges change)
    {
        List<UpdateLearnerApiPutResponse.LearningUpdateChanges> changes;

        if (!scenarioContext.TryGetValue(ChangesKey, out changes))
        {
            changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>();
        }

        changes.Add(change);

        scenarioContext.Set(changes, ChangesKey);
    }

    [Given(@"the details passed in are the same as the existing learner details")]
    public void GivenTheDetailsPassedInAreTheSameAsTheExistingLearnerDetails()
    {
        scenarioContext.Set(new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>(), ChangesKey); // an empty list will be returned to indicate no changes
    }

    [When(@"the learner is updated")]
    public async Task WhenTheLearnerIsUpdated()
    {
        ConfigureLearnerInnerApi();
        ConfigureEarningsInnerApiToRespondeOkToEverything();
        await CallUpdateLearnerEndpoint();
    }

    [Then(@"a (.*) update request is sent to the earnings domain")]
    public void ThenARequestIsSentToTheEarningsDomain(string updateRequestType)
    {
        var requestUrl = GetEarningsRequestUrl(updateRequestType);
        var requests = testContext.EarningsApi.MockServer.LogEntries;

        requests.Should().ContainSingle(request => request.RequestMessage.Url.Contains(requestUrl),
            $"Expected a request to {requestUrl} but found {requests.Count} requests instead.");
    }

    [Then(@"no changes are made to the learner")]
    public void ThenNoChangesAreMadeToTheLearner()
    {
        var requests = testContext.EarningsApi.MockServer.LogEntries;
        requests.Should().BeEmpty("Expected no requests to the earnings domain, but found some.");
    }

    [Then(@"sld data is stored to the cache")]
    public async Task ThenSldDataIsStoredToTheCache()
    {
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var sldLearnerData = scenarioContext.Get<UpdateLearnerRequest>(SldLearnerDataKey);
        var cachedData = await testContext.Cache.GetLearner(ukprn, sldLearnerData.Learner.Uln.ToString(), NullLogger.Instance, CancellationToken.None);

        cachedData.Should().NotBeNull();
        cachedData.Should().BeEquivalentTo(sldLearnerData);
    }

    [Given("the funding band maximum for that learner is set")]
    public void GivenTheFundingBandMaximumForThatApprenticeshipIsSet()
    {
        SetupFundingBandMaximum();
    }

    private void SetupFundingBandMaximum()
    {
        var fundingBandMaximum = _fixture.Create<int>();
        scenarioContext.Set(fundingBandMaximum, FundingBandMaximumKey);

        var response = new StandardDetailResponse
        {
            ApprenticeshipFunding =
            [
                new ApprenticeshipFunding
                {
                    EffectiveFrom = DateTime.MinValue,
                    EffectiveTo = DateTime.MaxValue,
                    MaxEmployerLevyCap = fundingBandMaximum
                }
            ]
        };

        testContext.CoursesApi.MockServer
            .Given(
                Request
                .Create()
                .WithPath($"/api/courses/standards/*"))
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithBodyAsJson(response));
    }

    private void ConfigureLearnerInnerApi()
    {
        var changes = scenarioContext.Get<List<UpdateLearnerApiPutResponse.LearningUpdateChanges>>(ChangesKey);
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);


        var response = new UpdateLearnerApiPutResponse();
        if (changes.Any())
        {
            response.Changes.AddRange(changes);
        }

        testContext.ApprenticeshipsApi.MockServer
        .Given(
            Request
            .Create()
            .WithPath($"/{learnerKey}")
            .UsingPut())
        .RespondWith(
            Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithBodyAsJson(response)
        );
    }

    private void ConfigureEarningsInnerApiToRespondeOkToEverything()
    {
        testContext.EarningsApi.MockServer
            .Given(
                Request.Create()
                    .UsingAnyMethod()
                    .WithPath(new WildcardMatcher("*")) // matches everything
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
            );
    }

    private async Task CallUpdateLearnerEndpoint()
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var requestBody = _fixture.Create<UpdateLearnerRequest>();
        var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), new MediaTypeHeaderValue("application/json"));
        var response = await testContext.OuterApiClient.PutAsync($"/providers/{ukprn}/learning/{learnerKey}", httpContent);
        var contentString = await response.Content.ReadAsStringAsync();
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer Api call, but got {response.StatusCode}. Content: {contentString}");

        scenarioContext.Set(requestBody, SldLearnerDataKey);
    }

    private string GetEarningsRequestUrl(string updateRequestType)
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);

        switch (updateRequestType)
        {
            case "on-programme":
                return $"learning/{learnerKey.ToString()}/on-programme";
            case "learning-support":
                return $"learning/{learnerKey.ToString()}/learning-support";
            case "english-and-maths":
                return $"learning/{learnerKey.ToString()}/english-and-maths";
            default:
                throw new ArgumentOutOfRangeException(nameof(updateRequestType), updateRequestType, null);
        }
    }
}
