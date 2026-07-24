using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
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
[Scope(Feature = "UpdateLearner")]
internal class UpdateLearnerSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private readonly Fixture _fixture = new Fixture();
    private const string ChangesKey = "Changes";
    private const string LearnerKey = "LearnerKey";
    private const string UkprnKey = "UkprnKey";
    private const string FundingBandMaximumKey = "FundingBandMaximumKey";
    private const string SldLearnerDataKey = "SldLearnerDataKey";
    private const string OuterApiResponseKey = "UpdateLearnerOuterApiResponse";

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

    [Given(@"the details passed in include multiple on-programme learnings")]
    public void GivenTheDetailsPassedInIncludeMultipleOnProgrammeLearnings()
    {
        scenarioContext.Set(new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>(), ChangesKey);

        var request = _fixture.Build<UpdateLearnerRequest>()
            .With(x => x.ConsumerReference, "update-learner-consumer-ref")
            .With(x => x.Delivery, new UpdateLearnerRequestDeliveryDetails
            {
                OnProgramme =
                [
                    _fixture.Build<OnProgrammeRequestDetails>()
                        .With(x => x.StandardCode, 300)
                        .With(x => x.PercentageOfTrainingLeft, 70)
                        .With(x => x.IsFlexiJob, true)
                        .With(x => x.Costs, [new CostDetails { TrainingPrice = 14000, EpaoPrice = 2500 }])
                        .Create(),
                    _fixture.Build<OnProgrammeRequestDetails>()
                        .With(x => x.StandardCode, 400)
                        .With(x => x.PercentageOfTrainingLeft, 35)
                        .With(x => x.IsFlexiJob, false)
                        .With(x => x.Costs, [new CostDetails { TrainingPrice = 8000, EpaoPrice = 2000 }])
                        .Create()
                ],
                EnglishAndMaths = []
            })
            .Create();

        scenarioContext.Set(request, SldLearnerDataKey);
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
        var cachedData = await testContext.Cache.GetLearner<UpdateLearnerRequest>(ukprn, sldLearnerData.Learner.Uln.ToString(), CancellationToken.None);

        cachedData.Should().NotBeNull();
        cachedData.Should().BeEquivalentTo(sldLearnerData);
    }

    [Then(@"approvals is informed of each on-programme learning")]
    public void ThenApprovalsIsInformedOfEachOnProgrammeLearning()
    {
        var response = scenarioContext.Get<HttpResponseMessage>(OuterApiResponseKey);
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var request = scenarioContext.Get<UpdateLearnerRequest>(SldLearnerDataKey);
        var publishedEvents = StubMessageSession.PublishedMessages.OfType<LearnerDataEvent>().ToList();

        publishedEvents.Count.Should().Be(request.Delivery.OnProgramme.Count);

        foreach (var onProgramme in request.Delivery.OnProgramme)
        {
            var evt = publishedEvents.Should().ContainSingle(x => x.StandardCode == onProgramme.StandardCode).Subject;
            var cost = onProgramme.Costs!.First();

            evt.ULN.Should().Be(request.Learner.Uln);
            evt.UKPRN.Should().Be(ukprn);
            evt.FirstName.Should().Be(request.Learner.FirstName);
            evt.LastName.Should().Be(request.Learner.LastName);
            evt.Email.Should().Be(request.Learner.Email);
            evt.DoB.Should().Be(request.Learner.Dob);
            evt.StartDate.Should().Be(onProgramme.StartDate);
            evt.PlannedEndDate.Should().Be(onProgramme.ExpectedEndDate);
            evt.PercentageLearningToBeDelivered.Should().Be(onProgramme.PercentageOfTrainingLeft);
            evt.EpaoPrice.Should().Be(cost.EpaoPrice ?? 0);
            evt.TrainingPrice.Should().Be(cost.TrainingPrice ?? 0);
            evt.AgreementId.Should().Be(onProgramme.AgreementId);
            evt.IsFlexiJob.Should().Be(onProgramme.IsFlexiJob.GetValueOrDefault());
            evt.ConsumerReference.Should().Be(request.ConsumerReference);
            evt.CorrelationId.Should().NotBe(Guid.Empty);
            evt.ReceivedDate.Should().NotBe(default);
            evt.LearningType.Should().Be(LearningType.Apprenticeship);
        }
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

        scenarioContext.Set(response);
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
        StubMessageSession.PublishedMessages.Clear();

        var learnerKey = scenarioContext.Get<Guid>(LearnerKey);
        var ukprn = scenarioContext.Get<long>(UkprnKey);
        if (!scenarioContext.TryGetValue(SldLearnerDataKey, out UpdateLearnerRequest requestBody))
        {
            requestBody = _fixture.Create<UpdateLearnerRequest>();
        }

        var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), new MediaTypeHeaderValue("application/json"));
        var response = await testContext.OuterApiClient.PutAsync($"/providers/{ukprn}/learning/{learnerKey}", httpContent);
        var contentString = await response.Content.ReadAsStringAsync();
        response.IsSuccessStatusCode.Should().BeTrue($"Expected successful response from outer Api call, but got {response.StatusCode}. Content: {contentString}");

        scenarioContext.Set(response, OuterApiResponseKey);
        scenarioContext.Set(requestBody, SldLearnerDataKey);
    }

    private string GetEarningsRequestUrl(string updateRequestType)
    {
        var learningKey = scenarioContext.Get<UpdateLearnerApiPutResponse>().LearningKey;

        switch (updateRequestType)
        {
            case "on-programme":
                return $"learning/{learningKey.ToString()}/on-programme";
            case "learning-support":
                return $"learning/{learningKey.ToString()}/learning-support";
            case "english-and-maths":
                return $"learning/{learningKey.ToString()}/english-and-maths";
            default:
                throw new ArgumentOutOfRangeException(nameof(updateRequestType), updateRequestType, null);
        }
    }
}
