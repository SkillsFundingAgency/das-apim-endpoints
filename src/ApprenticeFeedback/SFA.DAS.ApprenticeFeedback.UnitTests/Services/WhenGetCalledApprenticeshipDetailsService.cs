using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ApprenticeAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Services
{
    public class WhenGetCalledApprenticeshipDetailsService
    {
        [Test, MoqAutoData]
        public async Task Then_ShouldGetLearnerData(
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            ApprenticeshipDetailsService sut)
        {
            // Act
            var result = await sut.Get(Guid.NewGuid(), 1);

            // Assert
            mockAssesorsApiClient.Verify(p => p.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ShouldNotGetMyApprenticeshipData_WhenLearnerDataIsAvailable(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> mockApprenticeAccountsApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            var learnerResponse = new ApiResponse<GetApprenticeLearnerResponse>(new GetApprenticeLearnerResponse(), HttpStatusCode.OK, string.Empty);

            mockAssesorsApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ReturnsAsync(learnerResponse);

            // Act
            var result = await sut.Get(Guid.NewGuid(), 1);

            // Assert
            mockApprenticeAccountsApiClient.Verify(p => p.GetWithResponseCode<GetMyApprenticeshipResponse>(It.IsAny<GetMyApprenticeshipRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_ShouldGetMyApprenticeshipData_WhenLearnerDataIsNotAvailable(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> mockApprenticeAccountsApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            mockAssesorsApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ReturnsAsync(new ApiResponse<GetApprenticeLearnerResponse>(null, HttpStatusCode.OK, string.Empty));

            // Act
            var result = await sut.Get(Guid.NewGuid(), 1);

            // Assert
            mockApprenticeAccountsApiClient.Verify(p => p.GetWithResponseCode<GetMyApprenticeshipResponse>(It.IsAny<GetMyApprenticeshipRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ShouldReturnLearnerData_WhenApiResponseIsSuccessful(
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            var learnerResponse = new ApiResponse<GetApprenticeLearnerResponse>(new GetApprenticeLearnerResponse(), HttpStatusCode.OK, string.Empty);

            mockAssesorsApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ReturnsAsync(learnerResponse);

            // Act
            var result = await sut.Get(Guid.NewGuid(), 1);

            // Assert
            using (new AssertionScope())
            {
                result.LearnerData.Should().NotBeNull();
                result.MyApprenticeshipData.Should().BeNull();
            }
        }

        [Test, MoqAutoData]
        public async Task Then_ShouldHandleNonOkStatus_WhenRetrievingLearnerData(
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            var learnerResponse = new ApiResponse<GetApprenticeLearnerResponse>(null, HttpStatusCode.BadRequest, "Error");
            mockAssesorsApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ReturnsAsync(learnerResponse);

            // Act
            var result = await sut.Get(Guid.NewGuid(), 1);

            // Assert
            using (new AssertionScope())
            {
                result.LearnerData.Should().BeNull();
                result.MyApprenticeshipData.Should().BeNull();
            }
        }

        [Test, MoqAutoData]
        public async Task Then_ShouldReturnMyApprenticeshipData_WhenLearnerDataIsNull(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> mockApprenticeAccountsApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            var learnerResponse = new ApiResponse<GetApprenticeLearnerResponse>(null, HttpStatusCode.NoContent, string.Empty);
            var myApprenticeshipResponse = new ApiResponse<GetMyApprenticeshipResponse>(new GetMyApprenticeshipResponse(), HttpStatusCode.OK, string.Empty);

            mockAssesorsApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ReturnsAsync(learnerResponse);

            mockApprenticeAccountsApiClient
                .Setup(x => x.GetWithResponseCode<GetMyApprenticeshipResponse>(It.IsAny<GetMyApprenticeshipRequest>()))
                .ReturnsAsync(myApprenticeshipResponse);

            // Act
            var result = await sut.Get(Guid.NewGuid(), 1);

            // Assert
            using (new AssertionScope())
            {
                result.LearnerData.Should().BeNull();
                result.MyApprenticeshipData.Should().NotBeNull();
            }
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest, "Error")]
        [MoqInlineAutoData(HttpStatusCode.NotFound, "")]
        public async Task Then_ShouldHandleNonOkStatus_WhenRetrievingMyApprenticeshipData(
            HttpStatusCode httpStatusCode, string errorContent,
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> mockApprenticeAccountsApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            var learnerResponse = new ApiResponse<GetApprenticeLearnerResponse>(null, HttpStatusCode.NoContent, string.Empty);
            var myApprenticeshipResponse = new ApiResponse<GetMyApprenticeshipResponse>(null, httpStatusCode, errorContent);

            mockAssesorsApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ReturnsAsync(learnerResponse);

            mockApprenticeAccountsApiClient
                .Setup(x => x.GetWithResponseCode<GetMyApprenticeshipResponse>(It.IsAny<GetMyApprenticeshipRequest>()))
                .ReturnsAsync(myApprenticeshipResponse);

            // Act
            var result = await sut.Get(Guid.NewGuid(), 1);

            // Assert
            using (new AssertionScope())
            {
                result.LearnerData.Should().BeNull();
                result.MyApprenticeshipData.Should().BeNull();
            }
        }

        [Test, MoqAutoData]
        public async Task Then_ShouldLogError_WhenExceptionIsThrownFromAssessorClient(
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            [Frozen] Mock<ILogger<UpdateApprenticeFeedbackTargetCommandHandler>> mockLogger,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            mockAssesorsApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await sut.Get(Guid.NewGuid(), 1);

            // Assert
            VerifyErrorLog(mockLogger, LogLevel.Error, Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ShouldLogError_WhenExceptionIsThrownFromAccountsClient(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> mockApprenticeAccountsApiClient,
            [Frozen] Mock<ILogger<UpdateApprenticeFeedbackTargetCommandHandler>> mockLogger,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            mockApprenticeAccountsApiClient
                .Setup(x => x.GetWithResponseCode<GetMyApprenticeshipResponse>(It.IsAny<GetMyApprenticeshipRequest>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await sut.Get(Guid.NewGuid(), 1);

            // Assert
            VerifyErrorLog(mockLogger, LogLevel.Error, Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ShouldNotLogError_WhenMyApprenticeshipDataIsNotFound(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> mockApprenticeAccountsApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssesorsApiClient,
            [Frozen] Mock<ILogger<UpdateApprenticeFeedbackTargetCommandHandler>> mockLogger,
            ApprenticeshipDetailsService sut)
        {
            // Arrange
            var learnerResponse = new ApiResponse<GetApprenticeLearnerResponse>(null, HttpStatusCode.NoContent, string.Empty);
            var myApprenticeshipResponse = new ApiResponse<GetMyApprenticeshipResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockAssesorsApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ReturnsAsync(learnerResponse);

            mockApprenticeAccountsApiClient
                .Setup(x => x.GetWithResponseCode<GetMyApprenticeshipResponse>(It.IsAny<GetMyApprenticeshipRequest>()))
                .ReturnsAsync(myApprenticeshipResponse);

            // Act
            await sut.Get(Guid.NewGuid(), 1);

            // Assert
            VerifyErrorLog(mockLogger, LogLevel.Error, Times.Never);
        }

        private void VerifyErrorLog<T>(Mock<ILogger<T>> mockLogger, LogLevel level, Func<Times> times)
        {
            mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == level),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            times);
        }
    }
}
