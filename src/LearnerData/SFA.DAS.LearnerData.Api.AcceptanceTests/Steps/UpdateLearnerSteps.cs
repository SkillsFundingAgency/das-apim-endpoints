using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using System.Net;
using System.Net.Http.Headers;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
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
    private const string SubsequentOnProgrammeKey = "SubsequentOnProgrammeKey";
    private const string ApprovalCheckStatusCodeKey = "ApprovalCheckStatusCodeKey";
    private const string LearnerRefKey = "LearnerRefKey";
    private const string NeedsFurtherApprovalKey = "NeedsFurtherApprovalKey";
    private const string ApprovalsVerdictKey = "ApprovalsVerdictKey";

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

    [Given(@"the learner submits an OnProgramme item for a subsequent apprenticeship")]
    public void GivenTheLearnerSubmitsAnOnProgrammeItemForASubsequentApprenticeship()
    {
        var onProgramme = _fixture.Create<OnProgrammeRequestDetails>();
        scenarioContext.Set(onProgramme, SubsequentOnProgrammeKey);
        scenarioContext.Set(new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>(), ChangesKey); // not exercising earnings changes in these scenarios
    }

    [Given(@"that apprenticeship is already approved")]
    public void GivenThatApprenticeshipIsAlreadyApproved()
    {
        scenarioContext.Set(HttpStatusCode.OK, ApprovalCheckStatusCodeKey);
    }

    [Given(@"that apprenticeship is not yet approved")]
    public void GivenThatApprenticeshipIsNotYetApproved()
    {
        scenarioContext.Set(HttpStatusCode.NotFound, ApprovalCheckStatusCodeKey);

        testContext.CoursesApi.MockServer
            .Given(
                Request
                .Create()
                .WithPath($"/api/courses/standards/*"))
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithBodyAsJson(new StandardDetailResponse { ApprenticeshipType = "Apprenticeship" }));
    }

    [Given(@"the learner has a LearnerRef of ""(.*)""")]
    public void GivenTheLearnerHasALearnerRefOf(string learnerRef)
    {
        scenarioContext.Set(learnerRef, LearnerRefKey);
    }

    [Given(@"Learning reports that the change needs further approval")]
    public void GivenLearningReportsThatTheChangeNeedsFurtherApproval()
    {
        scenarioContext.Set(true, NeedsFurtherApprovalKey);
    }

    [Given(@"Approvals auto-approves the change")]
    public void GivenApprovalsAutoApprovesTheChange()
    {
        scenarioContext.Set("autoApproved", ApprovalsVerdictKey);
    }

    [Given(@"Approvals asks for employer approval of the change")]
    public void GivenApprovalsAsksForEmployerApprovalOfTheChange()
    {
        scenarioContext.Set("EmployerApprovalRequested", ApprovalsVerdictKey);
    }

    [Then(@"Approvals is asked about the start date change")]
    public void ThenApprovalsIsAskedAboutTheStartDateChange()
    {
        var learningKey = scenarioContext.Get<UpdateLearnerApiPutResponse>().LearningKey;

        var entry = testContext.CommitmentsApi.MockServer.LogEntries.Should()
            .ContainSingle(x => x.RequestMessage.Method == "PUT" && x.RequestMessage.Url.Contains($"/approvals/{learningKey}")).Subject;

        entry.RequestMessage.Body.Should().Contain("StartDate");
    }

    [Then(@"Approvals is not asked about the change")]
    public void ThenApprovalsIsNotAskedAboutTheChange()
    {
        testContext.CommitmentsApi.MockServer.LogEntries.Should().BeEmpty();
    }

    [Then(@"further approval needed is cleared in the learning domain")]
    public void ThenFurtherApprovalNeededIsClearedInTheLearningDomain()
    {
        var response = scenarioContext.Get<UpdateLearnerApiPutResponse>();

        var entry = testContext.ApprenticeshipsApi.MockServer.LogEntries.Should()
            .ContainSingle(x => x.RequestMessage.Method == "POST" && x.RequestMessage.Url.Contains("clear-further-approval-needed")).Subject;

        entry.RequestMessage.Url.Should().Contain($"/learning/{response.LearningKey}/episodes/{response.LearningEpisodeKey}/clear-further-approval-needed");
        entry.RequestMessage.Url.Should().Contain("learningType=Apprenticeship");
    }

    [Then(@"further approval needed is not cleared in the learning domain")]
    public void ThenFurtherApprovalNeededIsNotClearedInTheLearningDomain()
    {
        testContext.ApprenticeshipsApi.MockServer.LogEntries
            .Should().NotContain(x => x.RequestMessage.Url.Contains("clear-further-approval-needed"));
    }

    [Then(@"no update request is sent to the earnings domain")]
    public void ThenNoUpdateRequestIsSentToTheEarningsDomain()
    {
        testContext.EarningsApi.MockServer.LogEntries.Should().BeEmpty();
    }

    [Then(@"a LearnerDataEvent is published")]
    public void ThenALearnerDataEventIsPublished()
    {
        StubMessageSession.PublishedMessages.Should().ContainSingle(m => m is LearnerDataEvent);
    }

    [Then(@"no LearnerDataEvent is published")]
    public void ThenNoLearnerDataEventIsPublished()
    {
        StubMessageSession.PublishedMessages.Should().NotContain(m => m is LearnerDataEvent);
    }

    [When(@"the learner is updated")]
    public async Task WhenTheLearnerIsUpdated()
    {
        ConfigureLearnerInnerApi();
        ConfigureEarningsInnerApiToRespondOkToEverything();
        await CallUpdateLearnerEndpoint();
    }

    [When(@"the learner is updated with new earnings profile version")]
    public async Task WhenTheLearnerIsUpdatedWithNewEarningsProfileVersion()
    {
        ConfigureLearnerInnerApi();
        ConfigureEarningsInnerApiToRespondOkToEverything(true);
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

    [Then(@"the LearnerRef sent to the learning domain is ""(.*)""")]
    public void ThenTheLearnerRefSentToTheLearningDomainIs(string expectedLearnerRef)
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var requestUrl = $"/{ukprn}/{learnerKey}";

        var entry = testContext.ApprenticeshipsApi.MockServer.LogEntries
            .Single(request => request.RequestMessage.Url.Contains(requestUrl) && request.RequestMessage.Method == "PUT");

        var body = JsonConvert.DeserializeObject<UpdateLearningRequestBody>(entry.RequestMessage.Body);
        body!.Learner.LearnerRef.Should().Be(expectedLearnerRef);
    }

    [Then(@"the release-earnings request sent to the earnings domain has the learner key and ref ""(.*)""")]
    public void ThenTheReleaseEarningsRequestHasTheLearnerKeyAndRef(string expectedLearnerRef)
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);
        var learningKey = scenarioContext.Get<UpdateLearnerApiPutResponse>().LearningKey;
        var requestUrl = $"learning/{learningKey}/release-earnings";

        var entry = testContext.EarningsApi.MockServer.LogEntries
            .Single(request => request.RequestMessage.Url.Contains(requestUrl) && request.RequestMessage.Method == "POST");

        var body = JsonConvert.DeserializeObject<ReleaseEarningsRequest>(entry.RequestMessage.Body);
        body!.LearnerKey.Should().Be(learnerKey);
        body.LearnerRef.Should().Be(expectedLearnerRef);
    }

    [Then(@"sld data is stored to the cache")]
    public async Task ThenSldDataIsStoredToTheCache()
    {
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var sldLearnerData = scenarioContext.Get<UpdateLearnerRequest>(SldLearnerDataKey);
        var cachedData = await testContext.Cache.GetLearner<UpdateLearnerRequest>(ukprn, sldLearnerData.Learner.Uln.ToString(), CancellationToken.None);

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
        var ukprn = scenarioContext.Get<long>(UkprnKey);

        var response = new UpdateLearnerApiPutResponse();
        if (changes.Any())
        {
            response.Changes.AddRange(changes);
        }

        if (scenarioContext.TryGetValue(NeedsFurtherApprovalKey, out bool needsFurtherApproval) && needsFurtherApproval)
        {
            ConfigureApprovalFlow(response);
        }

        testContext.ApprenticeshipsApi.MockServer
        .Given(
            Request
            .Create()
            .WithPath($"/{ukprn}/{learnerKey}")
            .UsingPut())
        .RespondWith(
            Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithBodyAsJson(response)
        );

        var approvalCheckStatusCode = scenarioContext.TryGetValue(ApprovalCheckStatusCodeKey, out HttpStatusCode explicitStatusCode)
            ? explicitStatusCode
            : HttpStatusCode.OK;

        testContext.ApprenticeshipsApi.MockServer
        .Given(
            Request
            .Create()
            .WithPath($"/{ukprn}/apprenticeships")
            .UsingHead())
        .RespondWith(
            Response.Create()
            .WithStatusCode(approvalCheckStatusCode)
        );

        scenarioContext.Set(response);
    }

    private void ConfigureApprovalFlow(UpdateLearnerApiPutResponse response)
    {
        response.LearningKey = Guid.NewGuid();
        response.LearningEpisodeKey = Guid.NewGuid();
        response.ApprovalsApprenticeshipId = 12345;
        response.IsApproved = true;
        response.NeedsFurtherApproval = true;
        response.LearningType = LearningType.Apprenticeship;
        response.Prices =
        [
            new UpdateLearnerApiPutResponse.EpisodePrice
            {
                Key = Guid.NewGuid(),
                StartDate = new DateTime(2026, 9, 15),
                EndDate = new DateTime(2027, 9, 14),
                TrainingPrice = 9000m,
                EndPointAssessmentPrice = 1000m,
                TotalPrice = 10000m
            }
        ];

        testContext.ApprenticeshipsApi.MockServer
            .Given(Request.Create()
                .WithPath($"/learning/{response.LearningKey}/episodes/{response.LearningEpisodeKey}/clear-further-approval-needed")
                .UsingPost())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NoContent));

        var verdict = scenarioContext.Get<string>(ApprovalsVerdictKey);

        testContext.CommitmentsApi.MockServer
            .Given(Request.Create()
                .WithPath($"/approvals/{response.LearningKey}")
                .UsingPut())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithBodyAsJson(new
                {
                    changes = new[] { new { changeType = "StartDate", approvalStatus = verdict } },
                    prices = Array.Empty<object>()
                }));
    }

    private void ConfigureEarningsInnerApiToRespondOkToEverything(bool hasNewEarningsProfileVersionBeenGenerated = false)
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
                    .WithBody($"{{\"HasNewEarningsProfileVersionBeenGenerated\":{hasNewEarningsProfileVersionBeenGenerated.ToString().ToLower()}}}")
            );
    }

    private async Task CallUpdateLearnerEndpoint()
    {
        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var requestBody = _fixture.Create<UpdateLearnerRequest>();

        if (scenarioContext.TryGetValue(SubsequentOnProgrammeKey, out OnProgrammeRequestDetails onProgramme))
        {
            requestBody.Delivery.OnProgramme = [onProgramme];
        }

        if (scenarioContext.TryGetValue(LearnerRefKey, out string learnerRef))
        {
            requestBody.Learner.LearnerRef = learnerRef;
        }

        var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), new MediaTypeHeaderValue("application/json"));
        var response = await testContext.OuterApiClient.PutAsync($"/providers/{ukprn}/learning/{learnerKey}", httpContent);
        var contentString = await response.Content.ReadAsStringAsync();
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer Api call, but got {response.StatusCode}. Content: {contentString}");

        scenarioContext.Set(requestBody, SldLearnerDataKey);
    }

    private string GetEarningsRequestUrl(string updateRequestType)
    {
        var learningKey = scenarioContext.Get<UpdateLearnerApiPutResponse>().LearningKey;

        switch (updateRequestType)
        {
            case "on-programme":
                return $"learning/{learningKey.ToString()}/on-programme";
            case "english-and-maths":
                return $"learning/{learningKey.ToString()}/english-and-maths";
            case "release-earnings":
                return $"learning/{learningKey.ToString()}/release-earnings";
            default:
                throw new ArgumentOutOfRangeException(nameof(updateRequestType), updateRequestType, null);
        }
    }
}
