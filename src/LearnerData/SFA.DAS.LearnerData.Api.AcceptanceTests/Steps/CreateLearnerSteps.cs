using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses;
using System.Net;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
public class CreateLearnerSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private readonly Fixture _fixture = new Fixture();
    private const string UkprnKey = "CreateLearnerUkprnKey";
    private const string CreateLearnerRequestKey = "CreateLearnerRequestKey";
    private const string OuterApiResponseKey = "CreateLearnerOuterApiResponse";
    private const string CorrelationResponseKey = "CreateLearnerCorrelationResponse";

    [When(@"SLD makes a create learner request with multiple on-programme learnings")]
    public async Task WhenSldMakesACreateLearnerRequestWithMultipleOnProgrammeLearnings()
    {
        StubMessageSession.PublishedMessages.Clear();

        scenarioContext.Set(_fixture.Create<long>(), UkprnKey);

        var request = _fixture.Build<CreateLearnerRequest>()
            .With(x => x.ConsumerReference, "create-learner-consumer-ref")
            .With(x => x.Delivery, new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme =
                [
                    _fixture.Build<OnProgrammeRequestDetails>()
                        .With(x => x.StandardCode, 100)
                        .With(x => x.PercentageOfTrainingLeft, 80)
                        .With(x => x.IsFlexiJob, true)
                        .With(x => x.Costs, [new CostDetails { TrainingPrice = 12000, EpaoPrice = 3000 }])
                        .Create(),
                    _fixture.Build<OnProgrammeRequestDetails>()
                        .With(x => x.StandardCode, 200)
                        .With(x => x.PercentageOfTrainingLeft, 45)
                        .With(x => x.IsFlexiJob, false)
                        .With(x => x.Costs, [new CostDetails { TrainingPrice = 9000, EpaoPrice = 1500 }])
                        .Create()
                ],
                EnglishAndMaths = []
            })
            .Create();

        scenarioContext.Set(request, CreateLearnerRequestKey);

        var ukprn = scenarioContext.Get<long>(UkprnKey);

        var httpContent = new StringContent(JsonConvert.SerializeObject(request), new MediaTypeHeaderValue("application/json"));
        var response = await testContext.OuterApiClient.PostAsync($"/providers/{ukprn}/learners", httpContent);
        var content = await response.Content.ReadAsStringAsync();

        scenarioContext.Set(response, OuterApiResponseKey);

        if (response.IsSuccessStatusCode)
        {
            var correlationResponse = JsonConvert.DeserializeObject<CorrelationResponse>(content);
            scenarioContext.Set(correlationResponse!, CorrelationResponseKey);
        }
    }

    [Then(@"approvals is informed of each on-programme learning")]
    public void ThenApprovalsIsInformedOfEachOnProgrammeLearning()
    {
        var response = scenarioContext.Get<HttpResponseMessage>(OuterApiResponseKey);
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var ukprn = scenarioContext.Get<long>(UkprnKey);
        var request = scenarioContext.Get<CreateLearnerRequest>(CreateLearnerRequestKey);
        var correlationResponse = scenarioContext.Get<CorrelationResponse>(CorrelationResponseKey);
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
            evt.StartDate.Should().Be(onProgramme.StartDate);
            evt.PlannedEndDate.Should().Be(onProgramme.ExpectedEndDate);
            evt.PercentageLearningToBeDelivered.Should().Be(onProgramme.PercentageOfTrainingLeft);
            evt.EpaoPrice.Should().Be(cost.EpaoPrice ?? 0);
            evt.TrainingPrice.Should().Be(cost.TrainingPrice ?? 0);
            evt.AgreementId.Should().Be(onProgramme.AgreementId);
            evt.IsFlexiJob.Should().Be(onProgramme.IsFlexiJob!.Value);
            evt.ConsumerReference.Should().Be(request.ConsumerReference);
            evt.CorrelationId.Should().Be(correlationResponse.CorrelationId);
            evt.LearningType.Should().Be(LearningType.Apprenticeship);
        }
    }
}
