using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Requests.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class ApprovalsServiceTests
{
    private const int MaxAttempts = 4; // 1 initial attempt + 3 retries

    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _client;
    private Mock<ILogger<ApprovalsService>> _logger;
    private ApprovalsService _sut;

    [SetUp]
    public void SetUp()
    {
        _client = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _logger = new Mock<ILogger<ApprovalsService>>();
        _sut = new ApprovalsService(_client.Object, new ApprovalsRequestBuilder(), _logger.Object, TimeSpan.Zero);
    }

    // ---- request and response --------------------------------------------------------------

    [Test]
    public async Task Should_Send_The_Built_Request_To_Approvals()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts(("StartDate", "autoApproved"))));

        await _sut.RequestApproval(10001234, 9999999999, response);

        _client.Verify(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(
            It.Is<IPutApiRequest<ApprovalsRequestBody>>(r =>
                r.PutUrl == $"approvals/{response.LearningKey}"
                && r.Data.Changes.Single().ChangeType == "StartDate"
                && r.Data.ApprenticeshipId == 65472475)), Times.Once);
    }

    [Test]
    public async Task Should_Return_The_Changes_Approvals_Responded_With()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        var approvalsResponse = new ApprovalsResponse
        {
            Changes =
            [
                new ApprovalsChangeResult
                {
                    ChangeType = "StartDate",
                    ApprovalStatus = "autoApproved",
                    Data = new ApprovalsChangeData { Old = "2026-09-01", New = "2026-09-15" }
                }
            ]
        };
        SetupResponses(Reply(HttpStatusCode.Created, approvalsResponse));

        var outcome = await _sut.RequestApproval(10001234, 9999999999, response);

        outcome.AllAutoApproved.Should().BeTrue();
        outcome.Response.Should().BeSameAs(approvalsResponse);
    }

    // ---- verdicts --------------------------------------------------------------------------

    [TestCase("autoApproved")]
    [TestCase("AutoApproved")]
    [TestCase("AUTOAPPROVED")]
    public async Task Should_Allow_The_Update_When_Every_Change_Is_AutoApproved(string status)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate, BaseLearnerApiPutResponse.LearningUpdateChanges.Prices);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts(("StartDate", status), ("Prices", status))));

        var mayProceed = (await _sut.RequestApproval(10001234, 9999999999, response)).AllAutoApproved;

        mayProceed.Should().BeTrue();
    }

    [TestCase("pending")]
    [TestCase("EmployerApprovalRequested")]
    [TestCase("autoRejected")]
    [TestCase("AutoRejected")]
    [TestCase("somethingUnrecognised")]
    public async Task Should_Not_Allow_The_Update_When_Any_Change_Is_Not_AutoApproved(string otherStatus)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate, BaseLearnerApiPutResponse.LearningUpdateChanges.Prices);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts(("StartDate", "autoApproved"), ("Prices", otherStatus))));

        var mayProceed = (await _sut.RequestApproval(10001234, 9999999999, response)).AllAutoApproved;

        mayProceed.Should().BeFalse();
    }

    [Test]
    public async Task Should_Not_Allow_The_Update_When_Approvals_Returns_No_Verdicts()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts()));

        var mayProceed = (await _sut.RequestApproval(10001234, 9999999999, response)).AllAutoApproved;

        mayProceed.Should().BeFalse();
    }

    // ---- planned end date warning -----------------------------------------------------------

    [TestCase("pending", null)]
    [TestCase("employerApprovalRequested", null)]
    [TestCase("autoRejected", "Overlaps another apprenticeship")]
    [TestCase("somethingUnrecognised", null)]
    public async Task Should_Warn_When_A_Planned_End_Date_Is_Not_AutoApproved(string status, string? reason)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate);
        var approvalsResponse = new ApprovalsResponse
        {
            Changes = [new ApprovalsChangeResult { ChangeType = "PlannedEndDate", ApprovalStatus = status, Reason = reason }]
        };
        SetupResponses(Reply(HttpStatusCode.Created, approvalsResponse));

        await _sut.RequestApproval(10001234, 9999999999, response);

        _logger.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains("planned end date") &&
                state.ToString()!.Contains(response.LearningKey.ToString()) &&
                state.ToString()!.Contains(status) &&
                state.ToString()!.Contains(reason ?? string.Empty)),
            It.IsAny<Exception?>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [TestCase("autoApproved")]
    [TestCase("AutoApproved")]
    [TestCase("AUTOAPPROVED")]
    public async Task Should_Not_Warn_When_A_Planned_End_Date_Is_AutoApproved(string status)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts(("PlannedEndDate", status))));

        await _sut.RequestApproval(10001234, 9999999999, response);

        VerifyNoPlannedEndDateWarning();
    }

    [TestCase("pending")]
    [TestCase("autoRejected")]
    public async Task Should_Not_Warn_About_A_Start_Date_That_Is_Not_AutoApproved(string status)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts(("StartDate", status))));

        await _sut.RequestApproval(10001234, 9999999999, response);

        VerifyNoPlannedEndDateWarning();
    }

    // ---- other approvals pending ------------------------------------------------------------

    [TestCase(true)]
    [TestCase(false)]
    public async Task Should_Log_Whether_Other_Approvals_Are_Pending(bool pendingApprovalsExist)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate);
        var approvalsResponse = new ApprovalsResponse
        {
            Changes = [new ApprovalsChangeResult { ChangeType = "PlannedEndDate", ApprovalStatus = "autoApproved" }],
            PendingApprovalsExist = pendingApprovalsExist
        };
        SetupResponses(Reply(HttpStatusCode.Created, approvalsResponse));

        await _sut.RequestApproval(10001234, 9999999999, response);

        _logger.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains(response.LearningKey.ToString()) &&
                state.ToString()!.Contains($"other approvals pending = {pendingApprovalsExist}")),
            It.IsAny<Exception?>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    // ---- failures and retries --------------------------------------------------------------

    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.RequestTimeout)]
    [TestCase(HttpStatusCode.TooManyRequests)]
    public async Task Should_Retry_Transient_Status_Codes_Then_Succeed(HttpStatusCode transient)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(
            Reply(transient, null, "upstream unavailable"),
            Reply(HttpStatusCode.Created, Verdicts(("StartDate", "autoApproved"))));

        var mayProceed = (await _sut.RequestApproval(10001234, 9999999999, response)).AllAutoApproved;

        mayProceed.Should().BeTrue();
        VerifyCalls(Times.Exactly(2));
    }

    [Test]
    public async Task Should_Retry_When_The_Call_Throws_HttpRequestException()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        _client.SetupSequence(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(It.IsAny<IPutApiRequest<ApprovalsRequestBody>>()))
            .ThrowsAsync(new HttpRequestException("connection reset"))
            .ReturnsAsync(Reply(HttpStatusCode.Created, Verdicts(("StartDate", "autoApproved"))));

        var mayProceed = (await _sut.RequestApproval(10001234, 9999999999, response)).AllAutoApproved;

        mayProceed.Should().BeTrue();
        VerifyCalls(Times.Exactly(2));
    }

    [Test]
    public async Task Should_Retry_When_The_Call_Times_Out()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        _client.SetupSequence(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(It.IsAny<IPutApiRequest<ApprovalsRequestBody>>()))
            .ThrowsAsync(new TaskCanceledException("timed out"))
            .ReturnsAsync(Reply(HttpStatusCode.Created, Verdicts(("StartDate", "autoApproved"))));

        var mayProceed = (await _sut.RequestApproval(10001234, 9999999999, response)).AllAutoApproved;

        mayProceed.Should().BeTrue();
        VerifyCalls(Times.Exactly(2));
    }

    [Test]
    public async Task Should_Treat_As_Not_Approved_When_Transient_Failures_Are_Exhausted()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Enumerable.Repeat(Reply(HttpStatusCode.ServiceUnavailable, null, "service unavailable"), MaxAttempts).ToArray());

        var outcome = await _sut.RequestApproval(10001234, 9999999999, response);

        VerifyNotApproved(outcome);
        VerifyCalls(Times.Exactly(MaxAttempts));
        VerifyNoDecisionLogged(response);
    }

    [Test]
    public async Task Should_Treat_As_Not_Approved_When_The_Call_Keeps_Throwing_HttpRequestException()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        _client.Setup(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(It.IsAny<IPutApiRequest<ApprovalsRequestBody>>()))
            .ThrowsAsync(new HttpRequestException("connection reset"));

        var outcome = await _sut.RequestApproval(10001234, 9999999999, response);

        VerifyNotApproved(outcome);
        VerifyCalls(Times.Exactly(MaxAttempts));
        VerifyNoDecisionLogged(response);
    }

    [Test]
    public async Task Should_Treat_As_Not_Approved_When_The_Call_Keeps_Timing_Out()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        _client.Setup(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(It.IsAny<IPutApiRequest<ApprovalsRequestBody>>()))
            .ThrowsAsync(new TaskCanceledException("timed out"));

        var outcome = await _sut.RequestApproval(10001234, 9999999999, response);

        VerifyNotApproved(outcome);
        VerifyCalls(Times.Exactly(MaxAttempts));
        VerifyNoDecisionLogged(response);
    }

    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.Conflict)]
    public async Task Should_Treat_A_Permanent_Failure_As_Not_Approved_Without_Retrying(HttpStatusCode permanent)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(permanent, null, "rejected"));

        var outcome = await _sut.RequestApproval(10001234, 9999999999, response);

        VerifyNotApproved(outcome);
        VerifyCalls(Times.Once());
        VerifyNoDecisionLogged(response);
    }

    [Test]
    public async Task Should_Treat_An_Unreadable_Response_As_Not_Approved_Without_Retrying()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        _client.Setup(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(It.IsAny<IPutApiRequest<ApprovalsRequestBody>>()))
            .ThrowsAsync(new System.Text.Json.JsonException("unexpected token"));

        var outcome = await _sut.RequestApproval(10001234, 9999999999, response);

        VerifyNotApproved(outcome);
        VerifyCalls(Times.Once());
        VerifyNoDecisionLogged(response);
    }

    [Test]
    public async Task Should_Let_A_Failure_To_Build_The_Request_Propagate_Without_Calling_Approvals()
    {
        var builder = new Mock<IApprovalsRequestBuilder>();
        builder.Setup(x => x.Build(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<BaseLearnerApiPutResponse>()))
            .Throws(new InvalidOperationException("builder bug"));
        var sut = new ApprovalsService(_client.Object, builder.Object, _logger.Object, TimeSpan.Zero);
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);

        var act = () => sut.RequestApproval(10001234, 9999999999, response);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("builder bug");
        VerifyCalls(Times.Never());
    }

    // ---- helpers ---------------------------------------------------------------------------

    private void SetupResponses(params ApiResponse<ApprovalsResponse>[] responses)
    {
        // responses are returned in order, one per call
        var sequence = _client.SetupSequence(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(It.IsAny<IPutApiRequest<ApprovalsRequestBody>>()));

        foreach (var response in responses)
        {
            sequence = sequence.ReturnsAsync(response);
        }
    }

    private static void VerifyNotApproved(ApprovalsOutcome outcome)
    {
        outcome.AllAutoApproved.Should().BeFalse();
        outcome.Response.Changes.Should().BeEmpty();
    }

    private void VerifyNoDecisionLogged(BaseLearnerApiPutResponse response) =>
        _logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains(response.LearningKey.ToString()) &&
                state.ToString()!.Contains("treating it as not approved")),
            It.IsAny<Exception?>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

    private void VerifyNoPlannedEndDateWarning() =>
        _logger.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("planned end date")),
            It.IsAny<Exception?>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Never);

    private void VerifyCalls(Times times) =>
        _client.Verify(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(It.IsAny<IPutApiRequest<ApprovalsRequestBody>>()), times);

    private static ApiResponse<ApprovalsResponse> Reply(HttpStatusCode status, ApprovalsResponse? body, string errorContent = "") =>
        new(body!, status, errorContent);

    private static ApprovalsResponse Verdicts(params (string ChangeType, string Status)[] verdicts) => new()
    {
        Changes = verdicts.Select(v => new ApprovalsChangeResult { ChangeType = v.ChangeType, ApprovalStatus = v.Status }).ToList()
    };

    private static UpdateLearnerApiPutResponse ResponseWith(bool isApproved, params BaseLearnerApiPutResponse.LearningUpdateChanges[] changes) => new()
    {
        LearningKey = Guid.NewGuid(),
        ApprovalsApprenticeshipId = 65472475,
        IsApproved = isApproved,
        LearningType = LearningType.Apprenticeship,
        Changes = [.. changes],
        ChangeDetails = changes
            .Select(x => new BaseLearnerApiPutResponse.ChangeDetail
            {
                ChangeType = x,
                Old = x switch
                {
                    BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate => JsonValue.Create("2026-09-01"),
                    BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate => JsonValue.Create("2027-08-31"),
                    _ => null
                },
                New = x switch
                {
                    BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate => JsonValue.Create("2026-09-15"),
                    BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate => JsonValue.Create("2027-10-31"),
                    _ => null
                },
                RequiresApproval = true
            })
            .ToList(),
        Prices =
        [
            new BaseLearnerApiPutResponse.EpisodePrice
            {
                StartDate = new DateTime(2026, 9, 15),
                EndDate = new DateTime(2027, 8, 31),
                TrainingPrice = 15000m,
                EndPointAssessmentPrice = 2000m,
                TotalPrice = 17000m
            }
        ]
    };
}
