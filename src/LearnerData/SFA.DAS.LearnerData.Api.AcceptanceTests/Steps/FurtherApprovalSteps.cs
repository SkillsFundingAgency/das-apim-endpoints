using FluentAssertions;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.Learning.Types;
using SFA.DAS.LearnerData.Responses.LearningInner;
using System.Net;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

/// <summary>
/// Further approval steps shared by the update (PUT) and create (reinstating POST) features.
/// The mocked Learning response is held in the scenario context as a <see cref="BaseLearnerApiPutResponse"/>.
/// </summary>
[Binding]
internal class FurtherApprovalSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private const string NeedsFurtherApprovalKey = "NeedsFurtherApprovalKey";
    private const string ApprovalsVerdictKey = "ApprovalsVerdictKey";
    private const string ApprovalsEchoedOld = "2026-08-31";
    private const string ApprovalsEchoedNew = "2026-09-14";

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
        var learningKey = scenarioContext.Get<BaseLearnerApiPutResponse>().LearningKey;

        var entry = testContext.CommitmentsApi.MockServer.LogEntries.Should()
            .ContainSingle(x => x.RequestMessage.Method == "PUT" && x.RequestMessage.Url.Contains($"/approvals/{learningKey}")).Subject;

        entry.RequestMessage.Body.Should().Contain("StartDate");

        // The old value comes from Learning's change detail, and appears nowhere else in the body
        entry.RequestMessage.Body.Should().Contain("2026-09-01");
    }

    [Then(@"an ApprovedLearningUpdatedEvent is published for the start date change")]
    public void ThenAnApprovedLearningUpdatedEventIsPublishedForTheStartDateChange()
    {
        var response = scenarioContext.Get<BaseLearnerApiPutResponse>();

        var evt = StubMessageSession.PublishedMessages.OfType<ApprovedLearningUpdatedEvent>().Should().ContainSingle().Subject;

        evt.LearningKey.Should().Be(response.LearningKey);
        evt.ApprenticeshipId.Should().Be(response.ApprovalsApprenticeshipId);
        var change = evt.Changes.Should().ContainSingle().Subject;
        change.ChangeType.Should().Be("StartDate");
        change.Data.Old.Should().Be(ApprovalsEchoedOld);
        change.Data.New.Should().Be(ApprovalsEchoedNew);
    }

    [Then(@"no ApprovedLearningUpdatedEvent is published")]
    public void ThenNoApprovedLearningUpdatedEventIsPublished()
    {
        StubMessageSession.PublishedMessages.Should().NotContain(m => m is ApprovedLearningUpdatedEvent);
    }

    [Then(@"Approvals is not asked about the change")]
    public void ThenApprovalsIsNotAskedAboutTheChange()
    {
        testContext.CommitmentsApi.MockServer.LogEntries.Should().BeEmpty();
    }

    [Then(@"further approval needed is cleared in the learning domain")]
    public void ThenFurtherApprovalNeededIsClearedInTheLearningDomain()
    {
        var response = scenarioContext.Get<BaseLearnerApiPutResponse>();

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

    /// <summary>
    /// If the scenario said Learning reports a change needing further approval, makes the mocked Learning response say so,
    /// and mocks the Approvals ask and the clear to match.
    /// </summary>
    public void ConfigureApprovalFlowIfNeeded(BaseLearnerApiPutResponse response)
    {
        if (!scenarioContext.TryGetValue(NeedsFurtherApprovalKey, out bool needsFurtherApproval) || !needsFurtherApproval)
        {
            return;
        }

        response.LearningKey = Guid.NewGuid();
        response.LearningEpisodeKey = Guid.NewGuid();
        response.ApprovalsApprenticeshipId = 12345;
        response.IsApproved = true;
        // Learning sets the stored flag when it reports a change that requires approval
        response.FurtherApprovalNeeded = true;
        response.ChangeDetails =
        [
            new BaseLearnerApiPutResponse.ChangeDetail
            {
                ChangeType = BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate,
                Old = System.Text.Json.Nodes.JsonValue.Create("2026-09-01"),
                New = System.Text.Json.Nodes.JsonValue.Create("2026-09-15"),
                RequiresApproval = true
            }
        ];
        response.LearningType = LearningType.Apprenticeship;
        response.Prices =
        [
            new BaseLearnerApiPutResponse.EpisodePrice
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
                    // Old/New values deliberately differ from the ones mock-Learning reports, so a test can tell that the event echoes Approvals and not Learning.
                    changes = new[] { new { changeType = "StartDate", approvalStatus = verdict, data = new { old = ApprovalsEchoedOld, @new = ApprovalsEchoedNew } } },
                    prices = Array.Empty<object>()
                }));
    }
}
