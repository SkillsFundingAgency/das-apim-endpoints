using System.Net;
using System.Text.Json.Nodes;
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
    private ApprovalsService _sut;

    [SetUp]
    public void SetUp()
    {
        _client = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _sut = new ApprovalsService(_client.Object, new ApprovalsRequestBuilder(), Mock.Of<ILogger<ApprovalsService>>(), TimeSpan.Zero);
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
    public async Task Should_Return_The_Changes_And_Prices_Approvals_Responded_With()
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
            ],
            Prices = [new ApprovalsPrice { TrainingPrice = "15000.00", AssessmentPrice = "2000.00", EffectiveFrom = new DateTime(2026, 9, 15) }]
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
    public async Task Should_Throw_When_Transient_Failures_Are_Exhausted()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Enumerable.Repeat(Reply(HttpStatusCode.ServiceUnavailable, null, "service unavailable"), MaxAttempts).ToArray());

        var act = () => _sut.RequestApproval(10001234, 9999999999, response);

        var ex = await act.Should().ThrowAsync<HttpRequestContentException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        VerifyCalls(Times.Exactly(MaxAttempts));
    }

    [Test]
    public async Task Should_Not_Retry_A_Permanent_Failure()
    {
        var response = ResponseWith(isApproved: true, BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        SetupResponses(Reply(HttpStatusCode.BadRequest, null, "bad request"));

        var act = () => _sut.RequestApproval(10001234, 9999999999, response);

        var ex = await act.Should().ThrowAsync<HttpRequestContentException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        VerifyCalls(Times.Once());
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
                // Learning supplies the old and new start date; no other change type is mapped to a field change yet
                Old = x == BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate ? JsonValue.Create("2026-09-01") : null,
                New = x == BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate ? JsonValue.Create("2026-09-15") : null,
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
