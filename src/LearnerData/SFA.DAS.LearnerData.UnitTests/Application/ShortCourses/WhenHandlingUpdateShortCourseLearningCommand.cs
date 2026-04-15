using AutoFixture;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.UpdateShortCourse;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using Milestone = SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone;

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
    private Mock<IMessageSession> _messageSession;

    private UpdateShortCourseLearningCommand _command;
    private Guid _learningKey;
    private long _ukprn;
    private DateTime _completionDate;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<UpdateShortCourseLearningCommandHandler>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _calculateGrowthAndSkillsPaymentsEventBuilder = new Mock<ICalculateGrowthAndSkillsPaymentsEventBuilder>();
        _messageSession = new Mock<IMessageSession>();

        _handler = new UpdateShortCourseLearningCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _calculateGrowthAndSkillsPaymentsEventBuilder.Object,
            _messageSession.Object,
            new PaymentsConfiguration { PaymentsEndpoint = "test-payments-endpoint" });

        _learningKey = Guid.NewGuid();
        _ukprn = 12345678;
        _completionDate = new DateTime(2025, 12, 1);

        _command = new UpdateShortCourseLearningCommand
        {
            LearningKey = _learningKey,
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
    public async Task Then_CompletionDate_Is_Sent_To_Earnings_Api_When_There_Are_Changes()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learningKey,
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseLearningPutResponse>(learningResponse, HttpStatusCode.OK, string.Empty));

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
            LearningKey = _learningKey,
            Changes = []
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseLearningPutResponse>(learningResponse, HttpStatusCode.OK, string.Empty));

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
            LearningKey = _learningKey,
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseLearningPutResponse>(learningResponse, HttpStatusCode.OK, string.Empty));

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
            LearningKey = _learningKey,
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseLearningPutResponse>(learningResponse, HttpStatusCode.OK, string.Empty));

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
    public async Task Then_LearningComplete_Milestone_Is_Added_When_CompletionDate_Set_And_Milestone_Absent()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learningKey,
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseLearningPutResponse>(learningResponse, HttpStatusCode.OK, string.Empty));

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
                r.Data.Milestones.Contains(Milestone.LearningComplete))),
            Times.Once);
    }
}
