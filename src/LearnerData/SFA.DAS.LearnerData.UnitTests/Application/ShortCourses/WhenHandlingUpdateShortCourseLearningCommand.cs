using AutoFixture;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Application.UpdateShortCourse;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using System.Net;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

[TestFixture]
public class WhenHandlingUpdateShortCourseLearningCommand
{
    private Fixture _fixture = new Fixture();
    private UpdateShortCourseLearningCommandHandler _handler;
    private Mock<ILogger<UpdateShortCourseLearningCommandHandler>> _logger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<ICalculateGrowthAndSkillsPaymentsEventBuilder> _calculateGrowthAndSkillsPaymentsEventBuilder;
    private Mock<IUpdateShortCourseOnProgrammeEarningPutRequestBuilder> _updateShortCourseOnProgrammeEarningPutRequestBuilder;
    private Mock<IShortCourseLookupService> _shortCourseLookupService;
    private Mock<IMessageSession> _messageSession;

    private UpdateShortCourseLearningCommand _command;
    private Guid _learnerKey;
    private long _ukprn;
    private DateTime _completionDate;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<UpdateShortCourseLearningCommandHandler>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _calculateGrowthAndSkillsPaymentsEventBuilder = new Mock<ICalculateGrowthAndSkillsPaymentsEventBuilder>();
        _updateShortCourseOnProgrammeEarningPutRequestBuilder = new Mock<IUpdateShortCourseOnProgrammeEarningPutRequestBuilder>();
        _updateShortCourseOnProgrammeEarningPutRequestBuilder
            .Setup(x => x.Build(It.IsAny<ShortCourseOnProgramme>()))
            .Returns(new UpdateShortCourseOnProgrammeRequestBody { Milestones = [] });
        _shortCourseLookupService = new Mock<IShortCourseLookupService>();
        _messageSession = new Mock<IMessageSession>();

        _handler = new UpdateShortCourseLearningCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _calculateGrowthAndSkillsPaymentsEventBuilder.Object,
            _updateShortCourseOnProgrammeEarningPutRequestBuilder.Object,
            _shortCourseLookupService.Object,
            _messageSession.Object,
            new PaymentsConfiguration { PaymentsEndpoint = "test-payments-endpoint" });

        _learnerKey = Guid.NewGuid();
        _ukprn = 12345678;
        _completionDate = new DateTime(2025, 12, 1);

        _command = new UpdateShortCourseLearningCommand
        {
            LearnerKey = _learnerKey,
            Ukprn = _ukprn,
            Request = new ShortCourseRequest
            {
                Learner = new ShortCourseLearnerRequestDetails
                {
                    Uln = 1234567890,
                    FirstName = "Test",
                    LastName = "Learner"
                },
                Delivery = new ShortCourseDelivery
                {
                    OnProgramme =
                    [
                        new ShortCourseOnProgramme
                        {
                            CourseCode = "123",
                            StartDate = new DateTime(2025, 1, 1),
                            ExpectedEndDate = new DateTime(2025, 12, 31),
                            CompletionDate = _completionDate,
                            Milestones = []
                        }
                    ]
                }
            }
        };

        _calculateGrowthAndSkillsPaymentsEventBuilder
            .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<UpdateShortCourseLearningPutResponse>(), It.IsAny<ShortCourseEarningsResponse>()))
            .ReturnsAsync(_fixture.Create<CalculateGrowthAndSkillsPayments>());
    }

    [Test]
    public async Task Then_Earnings_Delete_Is_Called_For_Omitted_Learning()
    {
        // Arrange
        var removedLearningKey = Guid.NewGuid();
        var removedEpisodeKey = Guid.NewGuid();

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, List<UpdateShortCourseLearningPutResponse>>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<List<UpdateShortCourseLearningPutResponse>>(
                [
                    new UpdateShortCourseLearningPutResponse { LearningKey = _learnerKey, CourseCode = "123", Changes = [] },
                    new UpdateShortCourseLearningPutResponse { IsRemoved = true, LearningKey = removedLearningKey, UpdatedEpisodeKey = removedEpisodeKey, CourseCode = "TEST02" }
                ],
                HttpStatusCode.OK, string.Empty));

        _earningsApiClient
            .Setup(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
                It.IsAny<DeleteShortCourseEarningsRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseEarningsResponse>(
                new DeleteShortCourseEarningsResponse(), HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
                It.Is<DeleteShortCourseEarningsRequest>(r =>
                    r.LearningKey == removedLearningKey && r.EpisodeKey == removedEpisodeKey),
                It.IsAny<bool>()),
            Times.Once);
    }

    [Test]
    public async Task Then_CompletionDate_Is_Sent_To_Earnings_Api_When_There_Are_Changes()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            CourseCode = "123",
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, List<UpdateShortCourseLearningPutResponse>>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<List<UpdateShortCourseLearningPutResponse>>([learningResponse], HttpStatusCode.OK, string.Empty));

        _updateShortCourseOnProgrammeEarningPutRequestBuilder
            .Setup(x => x.Build(It.IsAny<ShortCourseOnProgramme>()))
            .Returns(new UpdateShortCourseOnProgrammeRequestBody { CompletionDate = _completionDate, Milestones = [] });

        var earningsResponse = _fixture.Create<UpdateShortCourseEarningPutResponse>();

        _earningsApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.IsAny<UpdateShortCourseOnProgrammeEarningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseEarningPutResponse>(earningsResponse, HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(It.Is<UpdateShortCourseOnProgrammeEarningPutRequest>(r =>
                r.Data.CompletionDate == _completionDate)),
            Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Api_Is_Not_Called_When_There_Are_No_Changes()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            CourseCode = "123",
            Changes = []
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, List<UpdateShortCourseLearningPutResponse>>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<List<UpdateShortCourseLearningPutResponse>>([learningResponse], HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.Put(It.IsAny<UpdateShortCourseOnProgrammeEarningPutRequest>()),
            Times.Never);
    }

    [Test]
    public async Task Then_CalculateGrowthAndSkillsPayments_Command_Is_Sent_To_Payments_Endpoint()
    {
        // Arrange
        var builtCommand = _fixture.Create<CalculateGrowthAndSkillsPayments>();

        _calculateGrowthAndSkillsPaymentsEventBuilder
            .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<UpdateShortCourseLearningPutResponse>(), It.IsAny<ShortCourseEarningsResponse>()))
            .ReturnsAsync(builtCommand);

        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            CourseCode = "123",
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, List<UpdateShortCourseLearningPutResponse>>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<List<UpdateShortCourseLearningPutResponse>>([learningResponse], HttpStatusCode.OK, string.Empty));

        _earningsApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.IsAny<UpdateShortCourseOnProgrammeEarningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseEarningPutResponse>(_fixture.Create<UpdateShortCourseEarningPutResponse>(), HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _messageSession.Verify(x =>
            x.Send(builtCommand, It.IsAny<SendOptions>()),
            Times.Once);
    }

    [Test]
    public async Task Then_GrowthAndSkillsPaymentsRecalculatedEvent_Is_Published()
    {
        // Arrange
        var builtCommand = _fixture.Create<CalculateGrowthAndSkillsPayments>();

        _calculateGrowthAndSkillsPaymentsEventBuilder
            .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<UpdateShortCourseLearningPutResponse>(), It.IsAny<ShortCourseEarningsResponse>()))
            .ReturnsAsync(builtCommand);

        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            CourseCode = "123",
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, List<UpdateShortCourseLearningPutResponse>>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<List<UpdateShortCourseLearningPutResponse>>([learningResponse], HttpStatusCode.OK, string.Empty));

        _earningsApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.IsAny<UpdateShortCourseOnProgrammeEarningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseEarningPutResponse>(_fixture.Create<UpdateShortCourseEarningPutResponse>(), HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _messageSession.Verify(x =>
            x.Publish(It.Is<GrowthAndSkillsPaymentsRecalculatedEvent>(e => e.Command == builtCommand), It.IsAny<PublishOptions>()),
            Times.Once);
    }

    [Test]
    public async Task Then_Builder_Body_Is_Passed_To_Earnings_Api()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            CourseCode = "123",
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, List<UpdateShortCourseLearningPutResponse>>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<List<UpdateShortCourseLearningPutResponse>>([learningResponse], HttpStatusCode.OK, string.Empty));

        var builtBody = new UpdateShortCourseOnProgrammeRequestBody
        {
            CompletionDate = _completionDate,
            Milestones = [Milestone.LearningComplete]
        };
        _updateShortCourseOnProgrammeEarningPutRequestBuilder
            .Setup(x => x.Build(It.IsAny<ShortCourseOnProgramme>()))
            .Returns(builtBody);

        _earningsApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.IsAny<UpdateShortCourseOnProgrammeEarningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseEarningPutResponse>(_fixture.Create<UpdateShortCourseEarningPutResponse>(), HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.Is<UpdateShortCourseOnProgrammeEarningPutRequest>(r => r.Data == builtBody)),
            Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Post_Is_Called_For_Reinstated_Unapproved_Learning()
    {
        // Arrange
        var learningKey = Guid.NewGuid();
        var episodeKey = Guid.NewGuid();

        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = learningKey,
            UpdatedEpisodeKey = episodeKey,
            CourseCode = "123",
            Changes = [ShortCourseUpdateChanges.Reinstated.ToString()],
            Episodes =
            [
                new LearningInnerShortCourseEpisode
                {
                    IsApproved = false,
                    Price = 2500m,
                    LearningType = "ApprenticeshipUnit"
                }
            ]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, List<UpdateShortCourseLearningPutResponse>>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<List<UpdateShortCourseLearningPutResponse>>([learningResponse], HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.Post(It.Is<PostCreateUnapprovedShortCourseLearningRequest>(r =>
                ((CreateUnapprovedShortCourseLearningRequest)r.Data).LearningKey == learningKey &&
                ((CreateUnapprovedShortCourseLearningRequest)r.Data).EpisodeKey == episodeKey)),
            Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Post_Is_NOT_Called_For_Reinstated_Approved_Learning()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            UpdatedEpisodeKey = Guid.NewGuid(),
            CourseCode = "123",
            Changes = [ShortCourseUpdateChanges.Reinstated.ToString()],
            Episodes =
            [
                new LearningInnerShortCourseEpisode
                {
                    IsApproved = true,
                    LearningType = "ApprenticeshipUnit"
                }
            ]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, List<UpdateShortCourseLearningPutResponse>>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<List<UpdateShortCourseLearningPutResponse>>([learningResponse], HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.Post(It.IsAny<PostCreateUnapprovedShortCourseLearningRequest>()),
            Times.Never);
    }
}
