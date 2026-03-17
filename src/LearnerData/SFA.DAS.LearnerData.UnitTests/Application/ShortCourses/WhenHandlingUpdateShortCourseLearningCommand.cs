using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.UpdateShortCourse;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

[TestFixture]
public class WhenHandlingUpdateShortCourseLearningCommand
{
    private UpdateShortCourseLearningCommandHandler _handler;
    private Mock<ILogger<UpdateShortCourseLearningCommandHandler>> _logger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;

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

        _handler = new UpdateShortCourseLearningCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object);

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

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.Put(It.Is<UpdateShortCourseOnProgrammeEarningPutRequest>(r =>
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
}
