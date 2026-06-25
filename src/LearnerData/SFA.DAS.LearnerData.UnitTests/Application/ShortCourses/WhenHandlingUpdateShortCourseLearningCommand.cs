using AutoFixture;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.UpdateShortCourse;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.LearnerData.Requests;
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
    private Mock<IUpdateShortCourseOnProgrammeEarningPutRequestBuilder> _updateShortCourseOnProgrammeEarningPutRequestBuilder;

    private UpdateShortCourseLearningCommand _command;
    private Guid _learnerKey;
    private long _ukprn;
    private DateTime _completionDate;
    private string _learnerRef;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<UpdateShortCourseLearningCommandHandler>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _updateShortCourseOnProgrammeEarningPutRequestBuilder = new Mock<IUpdateShortCourseOnProgrammeEarningPutRequestBuilder>();
        _updateShortCourseOnProgrammeEarningPutRequestBuilder
            .Setup(x => x.Build(It.IsAny<ShortCourseOnProgramme>(), It.IsAny<UpdateShortCourseLearningPutResponse>(), It.IsAny<long>()))
            .Returns((ShortCourseOnProgramme _, UpdateShortCourseLearningPutResponse learningResponse, long ukprn) =>
                new UpdateShortCourseOnProgrammeRequestBody
                {
                    Milestones = [],
                    LearnerKey = learningResponse.LearnerKey,
                    LearnerRef = learningResponse.Episodes
                        .Where(e => e.Ukprn == ukprn)
                        .OrderByDescending(e => e.StartDate)
                        .Select(e => e.LearnerRef)
                        .FirstOrDefault() ?? string.Empty
                });
        _handler = new UpdateShortCourseLearningCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _updateShortCourseOnProgrammeEarningPutRequestBuilder.Object);

        _learnerKey = Guid.NewGuid();
        _ukprn = 12345678;
        _completionDate = new DateTime(2025, 12, 1);
        _learnerRef = "learner-ref-1";

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
    }
    
    [Test]
    public async Task Then_CompletionDate_Is_Sent_To_Earnings_Api_When_There_Are_Changes()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            LearnerKey = _learnerKey,
            Changes = [ShortCourseUpdateChanges.CompletionDate.ToString()],
            Episodes =
            [
                new LearningInnerShortCourseEpisode
                {
                    Ukprn = _ukprn,
                    StartDate = new DateTime(2025, 1, 1),
                    LearnerRef = _learnerRef
                }
            ]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseLearningPutResponse>(learningResponse, HttpStatusCode.OK, string.Empty));

        _updateShortCourseOnProgrammeEarningPutRequestBuilder
            .Setup(x => x.Build(It.IsAny<ShortCourseOnProgramme>(), It.IsAny<UpdateShortCourseLearningPutResponse>(), It.IsAny<long>()))
            .Returns((ShortCourseOnProgramme _, UpdateShortCourseLearningPutResponse learningResponse, long ukprn) =>
                new UpdateShortCourseOnProgrammeRequestBody
                {
                    CompletionDate = _completionDate,
                    Milestones = [],
                    LearnerKey = learningResponse.LearnerKey,
                    LearnerRef = learningResponse.Episodes
                        .Where(e => e.Ukprn == ukprn)
                        .OrderByDescending(e => e.StartDate)
                        .Select(e => e.LearnerRef)
                        .FirstOrDefault() ?? string.Empty
                });

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
                r.Data.CompletionDate == _completionDate &&
                r.Data.LearnerKey == _learnerKey &&
                r.Data.LearnerRef == _learnerRef)),
            Times.Once);
    }

    [Test]
    public async Task Then_WithdrawalReasonCode_Is_Sent_To_Learning_Api()
    {
        // Arrange
        const short withdrawalReasonCode = 5;
        _command.Request.Delivery.OnProgramme[0].WithdrawalReasonCode = withdrawalReasonCode;

        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            Changes = []
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseLearningPutResponse>(learningResponse, HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.Is<UpdateShortCourseLearningPutRequest>(r =>
                    r.Data.OnProgramme.WithdrawalReasonCode == withdrawalReasonCode)),
            Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Api_Is_Not_Called_When_There_Are_No_Changes()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
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
            x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.IsAny<UpdateShortCourseOnProgrammeEarningPutRequest>()),
            Times.Never);
    }

    [Test]
    public async Task Then_Builder_Body_Is_Passed_To_Earnings_Api()
    {
        // Arrange
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearningKey = _learnerKey,
            LearnerKey = _learnerKey,
            Changes = [nameof(ShortCourseUpdateChanges.CompletionDate)],
            Episodes =
            [
                new LearningInnerShortCourseEpisode
                {
                    Ukprn = _ukprn,
                    StartDate = new DateTime(2025, 1, 1),
                    LearnerRef = _learnerRef
                }
            ]
        };

        _learningApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseLearningRequestBody, UpdateShortCourseLearningPutResponse>(
                It.IsAny<UpdateShortCourseLearningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseLearningPutResponse>(learningResponse, HttpStatusCode.OK, string.Empty));

        var builtBody = new UpdateShortCourseOnProgrammeRequestBody
        {
            CompletionDate = _completionDate,
            Milestones = [Milestone.LearningComplete],
            LearnerKey = _learnerKey,
            LearnerRef = _learnerRef
        };
        _updateShortCourseOnProgrammeEarningPutRequestBuilder
            .Setup(x => x.Build(It.IsAny<ShortCourseOnProgramme>(), It.IsAny<UpdateShortCourseLearningPutResponse>(), It.IsAny<long>()))
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
                It.Is<UpdateShortCourseOnProgrammeEarningPutRequest>(r =>
                    r.Data == builtBody &&
                    r.Data.LearnerKey == _learnerKey &&
                    r.Data.LearnerRef == _learnerRef)),
            Times.Once);
    }
}
