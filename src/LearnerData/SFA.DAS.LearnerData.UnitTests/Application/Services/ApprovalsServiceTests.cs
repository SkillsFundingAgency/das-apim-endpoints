using System.Net;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure;
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
    private CapturingLogger _logger;
    private ApprovalsService _sut;

    [SetUp]
    public void SetUp()
    {
        _client = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _logger = new CapturingLogger();
        _sut = new ApprovalsService(_client.Object, new ApprovalsRequestBuilder(), _logger, TimeSpan.Zero);
    }

    // ---- the caller decides whether an ask is needed ----------------------------------------

    [Test]
    public async Task Should_Always_Ask_Approvals_Because_The_Caller_Decides_Whether_An_Ask_Is_Needed()
    {
        var response = ResponseWith(isApproved: false, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts(("StartDate", "autoApproved"))));

        await _sut.RequestApproval(10001234, 9999999999, response);

        VerifyCalls(Times.Once());
    }

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

    // ---- verdicts --------------------------------------------------------------------------

    [TestCase("autoApproved")]
    [TestCase("AutoApproved")]
    [TestCase("AUTOAPPROVED")]
    public async Task Should_Allow_The_Update_When_Every_Change_Is_AutoApproved(string status)
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate, BaseLearnerApiPutResponse.LearningUpdateChanges.Prices);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts(("StartDate", status), ("Prices", status))));

        var mayProceed = await _sut.RequestApproval(10001234, 9999999999, response);

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

        var mayProceed = await _sut.RequestApproval(10001234, 9999999999, response);

        mayProceed.Should().BeFalse();
    }

    [Test]
    public async Task Should_Not_Allow_The_Update_When_Approvals_Returns_No_Verdicts()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(HttpStatusCode.Created, Verdicts()));

        var mayProceed = await _sut.RequestApproval(10001234, 9999999999, response);

        mayProceed.Should().BeFalse();
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

        var mayProceed = await _sut.RequestApproval(10001234, 9999999999, response);

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

        var mayProceed = await _sut.RequestApproval(10001234, 9999999999, response);

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

        var mayProceed = await _sut.RequestApproval(10001234, 9999999999, response);

        mayProceed.Should().BeTrue();
        VerifyCalls(Times.Exactly(2));
    }

    [Test]
    public async Task Should_Throw_And_Log_The_Error_Body_When_Transient_Failures_Are_Exhausted()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(HttpStatusCode.ServiceUnavailable, null, "approvals is down"));

        var act = () => _sut.RequestApproval(10001234, 9999999999, response);

        var ex = await act.Should().ThrowAsync<HttpRequestContentException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        VerifyCalls(Times.Exactly(MaxAttempts));
        _logger.Entries.Should().Contain(e => e.Level == LogLevel.Error && e.Message.Contains("approvals is down"));
    }

    [Test]
    public async Task Should_Not_Retry_A_Permanent_Failure()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(HttpStatusCode.BadRequest, null, "ChangeType must be TNP1 or PlannedEndDate"));

        var act = () => _sut.RequestApproval(10001234, 9999999999, response);

        var ex = await act.Should().ThrowAsync<HttpRequestContentException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        VerifyCalls(Times.Once());
        _logger.Entries.Should().Contain(e => e.Level == LogLevel.Error && e.Message.Contains("ChangeType must be TNP1 or PlannedEndDate"));
    }

    // ---- logging ---------------------------------------------------------------------------

    [Test]
    public async Task Should_Log_Each_Change_Requested_And_Each_Outcome_In_Detail()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate, BaseLearnerApiPutResponse.LearningUpdateChanges.Prices);
        SetupResponses(Reply(HttpStatusCode.Created, new ApprovalsResponse
        {
            Changes =
            [
                new ApprovalsChangeResult { ChangeType = "StartDate", ApprovalStatus = "autoApproved" },
                new ApprovalsChangeResult { ChangeType = "Prices", ApprovalStatus = "pending", Reason = "needs employer sign-off" }
            ]
        }));

        await _sut.RequestApproval(10001234, 9999999999, response);

        var messages = _logger.Entries.Select(e => e.Message).ToList();

        // what was asked
        messages.Should().Contain(m => m.Contains("StartDate") && m.Contains("2026-09-15"));
        messages.Should().Contain(m => m.Contains("15000.00") && m.Contains("2000.00") && m.Contains("2026-09-15"));

        // every outcome, including the reason
        messages.Should().Contain(m => m.Contains("StartDate") && m.Contains("autoApproved"));
        messages.Should().Contain(m => m.Contains("Prices") && m.Contains("pending") && m.Contains("needs employer sign-off"));

        // the identifiers needed to trace it, but not the ULN
        messages.Should().Contain(m => m.Contains(response.LearningKey.ToString()) && m.Contains("65472475"));
        messages.Should().NotContain(m => m.Contains("9999999999"));
    }

    [Test]
    public async Task Should_Log_A_Warning_For_Each_Retry()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(
            Reply(HttpStatusCode.ServiceUnavailable, null, "approvals is down"),
            Reply(HttpStatusCode.Created, Verdicts(("StartDate", "autoApproved"))));

        await _sut.RequestApproval(10001234, 9999999999, response);

        _logger.Entries.Should().ContainSingle(e => e.Level == LogLevel.Warning && e.Message.Contains("503"));
    }

    // ---- helpers ---------------------------------------------------------------------------

    private void SetupResponses(params ApiResponse<ApprovalsResponse>[] responses)
    {
        // responses are returned in order, and the last one repeats for any further calls
        var call = 0;
        _client.Setup(x => x.PutWithResponseCode<ApprovalsRequestBody, ApprovalsResponse>(It.IsAny<IPutApiRequest<ApprovalsRequestBody>>()))
            .ReturnsAsync(() => responses[Math.Min(call++, responses.Length - 1)]);
    }

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
        ChangesNeedingApproval = [.. changes],
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

    private class CapturingLogger : ILogger<ApprovalsService>
    {
        public List<(LogLevel Level, string Message)> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            Entries.Add((logLevel, formatter(state, exception)));
    }
}
