using System.Net;
using AutoFixture;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.Learning.Types;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class FurtherApprovalServiceTests
{
    private const long Ukprn = 10005077;
    private const long Uln = 1234567890;

    private Fixture _fixture;

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
    private Mock<ILogger<FurtherApprovalService>> _logger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IApprovalsService> _approvalsService;
    private Mock<IApprovedLearningUpdatedEventBuilder> _approvedLearningUpdatedEventBuilder;
    private Mock<IMessageSession> _messageSession;
    private FurtherApprovalService _sut;
#pragma warning restore CS8618 // Non-nullable field, instantiated in SetUp method

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Customize(new JsonNodeCustomization());
        _logger = new Mock<ILogger<FurtherApprovalService>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _approvalsService = new Mock<IApprovalsService>();
        _approvedLearningUpdatedEventBuilder = new Mock<IApprovedLearningUpdatedEventBuilder>();
        _messageSession = new Mock<IMessageSession>();

        _sut = new FurtherApprovalService(
            _logger.Object,
            _learningApiClient.Object,
            _approvalsService.Object,
            _approvedLearningUpdatedEventBuilder.Object,
            _messageSession.Object);

        _approvalsService
            .Setup(x => x.RequestApproval(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<BaseLearnerApiPutResponse>()))
            .ReturnsAsync(new ApprovalsOutcome(true, new ApprovalsResponse()));

        _approvedLearningUpdatedEventBuilder
            .Setup(x => x.Build(It.IsAny<BaseLearnerApiPutResponse>(), It.IsAny<ApprovalsResponse>()))
            .Returns(_fixture.Create<ApprovedLearningUpdatedEvent>());

        _learningApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<ClearFurtherApprovalNeededApiPostRequest>(), false))
            .ReturnsAsync(new ApiResponse<object>(null!, HttpStatusCode.NoContent, string.Empty));
    }

    [Test]
    public async Task Then_Approval_Is_Requested_For_The_Changes_Learning_Reported()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();

        // Act
        await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        _approvalsService.Verify(x => x.RequestApproval(Ukprn, Uln, learningResponse), Times.Once);
    }

    [Test]
    public async Task Then_Further_Approval_Is_Required_If_Approval_Is_Not_Granted()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        _approvalsService
            .Setup(x => x.RequestApproval(Ukprn, Uln, learningResponse))
            .ReturnsAsync(new ApprovalsOutcome(false, new ApprovalsResponse()));

        // Act
        var result = await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        result.Should().BeTrue();
        VerifyNotCleared();
    }

    [Test]
    public async Task Then_Further_Approval_Is_Not_Required_If_Approval_Is_Granted_And_Cleared()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();

        // Act
        var result = await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        result.Should().BeFalse();
        _learningApiClient.Verify(x => x.PostWithResponseCode<object>(
            It.Is<ClearFurtherApprovalNeededApiPostRequest>(r => r.PostUrl ==
                $"learning/{learningResponse.LearningKey}/episodes/{learningResponse.LearningEpisodeKey}/clear-further-approval-needed?learningType={learningResponse.LearningType}"),
            false), Times.Once);
    }

    [Test]
    public async Task Then_Further_Approval_Is_Required_If_Approval_Is_Granted_But_Clearing_Fails()
    {
        // Arrange: Learning still holds the flag, so Earnings must withhold payments too
        var learningResponse = ArrangeStartDateChange();
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<ClearFurtherApprovalNeededApiPostRequest>(), false))
            .ReturnsAsync(new ApiResponse<object>(null!, HttpStatusCode.NotFound, "not found"));

        // Act
        var result = await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task Then_The_Exception_Is_Propagated_And_Nothing_Is_Cleared_If_Approvals_Fails()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        _approvalsService
            .Setup(x => x.RequestApproval(Ukprn, Uln, learningResponse))
            .ThrowsAsync(new HttpRequestException("approvals is down"));

        // Act
        Func<Task> act = async () => await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
        VerifyNotCleared();
    }

    [Test]
    public async Task Then_ApprovedLearningUpdatedEvent_Is_Published_After_FurtherApprovalNeeded_Is_Cleared()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        var evt = _fixture.Create<ApprovedLearningUpdatedEvent>();
        var calls = new List<string>();

        var approvalsResponse = new ApprovalsResponse
        {
            Changes = [new ApprovalsChangeResult { ChangeType = "StartDate", ApprovalStatus = "autoApproved" }]
        };
        _approvalsService
            .Setup(x => x.RequestApproval(Ukprn, Uln, learningResponse))
            .ReturnsAsync(new ApprovalsOutcome(true, approvalsResponse));
        _approvedLearningUpdatedEventBuilder.Setup(x => x.Build(learningResponse, approvalsResponse)).Returns(evt);
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<ClearFurtherApprovalNeededApiPostRequest>(), false))
            .Callback(() => calls.Add("clear"))
            .ReturnsAsync(new ApiResponse<object>(null!, HttpStatusCode.NoContent, string.Empty));
        _messageSession
            .Setup(x => x.Publish(evt, It.IsAny<PublishOptions>()))
            .Callback(() => calls.Add("publish"))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        _messageSession.Verify(x => x.Publish(evt, It.IsAny<PublishOptions>()), Times.Once);
        calls.Should().Equal("clear", "publish");
    }

    [Test]
    public async Task Then_The_Failure_Is_Logged_And_Not_Thrown_If_Publishing_ApprovedLearningUpdatedEvent_Throws()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        _messageSession
            .Setup(x => x.Publish(It.IsAny<ApprovedLearningUpdatedEvent>(), It.IsAny<PublishOptions>()))
            .ThrowsAsync(new InvalidOperationException("service bus is down"));

        // Act
        var result = await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert: the flag was cleared, so no further approval is required even though the event was not sent
        result.Should().BeFalse();

        // The log must identify the learning well enough for the event to be republished by hand
        _logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains(learningResponse.LearningKey.ToString()) &&
                state.ToString()!.Contains(learningResponse.ApprovalsApprenticeshipId.ToString())),
            It.IsAny<InvalidOperationException>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public async Task Then_Approvals_Is_Not_Asked_If_Learning_Says_Further_Approval_Is_Not_Needed()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange(needsFurtherApproval: false);

        // Act
        var result = await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        result.Should().BeFalse();
        _approvalsService.VerifyNoOtherCalls();
        VerifyNotCleared();
    }

    [Test]
    public async Task Then_Further_Approval_Is_Required_If_An_Earlier_Change_Is_Still_Awaiting_Approval()
    {
        // Arrange: this update is a withdrawal that needs no approval, but the episode is still flagged from an earlier one
        var learningResponse = ArrangeStartDateChange(needsFurtherApproval: false);
        learningResponse.ChangeDetails.Single().ChangeType = UpdateLearnerApiPutResponse.LearningUpdateChanges.Withdrawal;
        learningResponse.FurtherApprovalNeeded = true;

        // Act
        var result = await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        result.Should().BeTrue();
        _approvalsService.VerifyNoOtherCalls();
        VerifyNotCleared();
    }

    [Test]
    public async Task Then_The_Failure_Is_Logged_And_Not_Thrown_If_Clearing_FurtherApprovalNeeded_Throws()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<ClearFurtherApprovalNeededApiPostRequest>(), false))
            .ThrowsAsync(new HttpRequestException("learning is down"));

        // Act
        var result = await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        result.Should().BeTrue();
        VerifyErrorLogged(learningResponse);
    }

    [Test]
    public async Task Then_The_Failure_Is_Logged_And_Not_Thrown_If_Clearing_FurtherApprovalNeeded_Returns_An_Error_Status()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<ClearFurtherApprovalNeededApiPostRequest>(), false))
            .ReturnsAsync(new ApiResponse<object>(null!, HttpStatusCode.NotFound, "not found"));

        // Act
        var result = await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        result.Should().BeTrue();
        VerifyErrorLogged(learningResponse);
    }

    [Test]
    public async Task Then_ApprovedLearningUpdatedEvent_Is_Not_Published_If_Approval_Is_Not_Granted()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        _approvalsService
            .Setup(x => x.RequestApproval(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<BaseLearnerApiPutResponse>()))
            .ReturnsAsync(new ApprovalsOutcome(false, new ApprovalsResponse()));

        // Act
        await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        VerifyNotPublished();
    }

    [Test]
    public async Task Then_ApprovedLearningUpdatedEvent_Is_Not_Published_If_Learning_Says_Further_Approval_Is_Not_Needed()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange(needsFurtherApproval: false);

        // Act
        await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        VerifyNotPublished();
    }

    [Test]
    public async Task Then_ApprovedLearningUpdatedEvent_Is_Not_Published_If_Clearing_FurtherApprovalNeeded_Throws()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<ClearFurtherApprovalNeededApiPostRequest>(), false))
            .ThrowsAsync(new HttpRequestException("learning is down"));

        // Act
        await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        VerifyNotPublished();
    }

    [Test]
    public async Task Then_ApprovedLearningUpdatedEvent_Is_Not_Published_If_Clearing_FurtherApprovalNeeded_Returns_An_Error_Status()
    {
        // Arrange
        var learningResponse = ArrangeStartDateChange();
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<ClearFurtherApprovalNeededApiPostRequest>(), false))
            .ReturnsAsync(new ApiResponse<object>(null!, HttpStatusCode.NotFound, "not found"));

        // Act
        await _sut.Resolve(Ukprn, Uln, learningResponse);

        // Assert
        VerifyNotPublished();
    }

    private UpdateLearnerApiPutResponse ArrangeStartDateChange(bool needsFurtherApproval = true)
    {
        var learningResponse = _fixture.Create<UpdateLearnerApiPutResponse>();
        learningResponse.ChangeDetails =
        [
            new UpdateLearnerApiPutResponse.ChangeDetail
            {
                ChangeType = UpdateLearnerApiPutResponse.LearningUpdateChanges.StartDate,
                RequiresApproval = needsFurtherApproval
            }
        ];
        // Learning sets the persisted flag when it reports a change that requires approval
        learningResponse.FurtherApprovalNeeded = needsFurtherApproval;
        return learningResponse;
    }

    private void VerifyNotCleared()
    {
        _learningApiClient.Verify(x => x.PostWithResponseCode<object>(It.IsAny<ClearFurtherApprovalNeededApiPostRequest>(), It.IsAny<bool>()), Times.Never);
    }

    private void VerifyNotPublished()
    {
        _messageSession.Verify(x => x.Publish(It.IsAny<ApprovedLearningUpdatedEvent>(), It.IsAny<PublishOptions>()), Times.Never);
    }

    private void VerifyErrorLogged(UpdateLearnerApiPutResponse learningResponse)
    {
        _logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains(learningResponse.LearningKey.ToString()) &&
                state.ToString()!.Contains(learningResponse.LearningEpisodeKey.ToString())),
            It.IsAny<Exception?>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
