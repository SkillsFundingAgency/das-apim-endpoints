using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Commands
{
    public class WhenHandlingUpdateApprenticeFeedbackTarget
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockFeedbackApiClient;
        private Mock<IAssessorsApiClient<AssessorsApiConfiguration>> _mockAssessorApiClient;
        private UpdateApprenticeFeedbackTargetCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockFeedbackApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _mockAssessorApiClient = new Mock<IAssessorsApiClient<AssessorsApiConfiguration>>();

            _handler = new UpdateApprenticeFeedbackTargetCommandHandler(
                _mockFeedbackApiClient.Object, _mockAssessorApiClient.Object, Mock.Of<ILogger<UpdateApprenticeFeedbackTargetCommandHandler>>());
        }

        [Test, MoqAutoData]
        public async Task Then_RequestForAllApprenticeFeedbackTargetsIsSent(UpdateApprenticeFeedbackTargetCommand command)
        {
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.GetAll<ApprenticeFeedbackTarget>(It.Is<GetAllApprenticeFeedbackTargetsRequest>(x => x.ApprenticeId == command.ApprenticeId)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_RequestForAllApprenticeFeedbackTargetsIsSent_ButNoResults_ReturnsEmptyResponse(UpdateApprenticeFeedbackTargetCommand command)
        {
            // Arrange
            var response = new List<ApprenticeFeedbackTarget>();
            _mockFeedbackApiClient.Setup(c => c.GetAll<ApprenticeFeedbackTarget>(It.Is<GetAllApprenticeFeedbackTargetsRequest>(x => x.ApprenticeId == command.ApprenticeId))).ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.GetAll<ApprenticeFeedbackTarget>(It.Is<GetAllApprenticeFeedbackTargetsRequest>(x => x.ApprenticeId == command.ApprenticeId)), Times.Once);
            result.Should().BeOfType<UpdateApprenticeFeedbackTargetResponse>();
        }

        [Test, MoqAutoData]
        public async Task Then_RequestForLearnersCount_EqualsReturnedApprenticeFeedbackTargets(
            UpdateApprenticeFeedbackTargetCommand command,
            IEnumerable<ApprenticeFeedbackTarget> feedbackApiResponse,
            GetApprenticeLearnerResponse apprenticeLearnerResponse,
            ApprenticeFeedbackTarget apprenticeFeedbackTarget)
        {
            // Arrange
            var apiResponse = new ApiResponse<GetApprenticeLearnerResponse>(apprenticeLearnerResponse, HttpStatusCode.OK, string.Empty);
            _mockFeedbackApiClient.Setup(c => c.GetAll<ApprenticeFeedbackTarget>(It.Is<GetAllApprenticeFeedbackTargetsRequest>(x => x.ApprenticeId == command.ApprenticeId))).ReturnsAsync(feedbackApiResponse);
            // Don't care about responses here, just to pass the test.
            _mockAssessorApiClient.Setup(c => c.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>())).ReturnsAsync(apiResponse);

            _mockFeedbackApiClient.Setup(a => a.PostWithResponseCode<ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(apprenticeFeedbackTarget, HttpStatusCode.OK, string.Empty));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            foreach (var target in feedbackApiResponse)
            {
                _mockAssessorApiClient.Verify(s => s.GetWithResponseCode<GetApprenticeLearnerResponse>(It.Is<GetApprenticeLearnerRequest>(s => s.ApprenticeCommitmentsId == target.ApprenticeshipId)), Times.Once);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_PostForUpdateOfTarget_EqualsReturnedApprenticeFeedbackTargetsWithLearnerData(
            UpdateApprenticeFeedbackTargetCommand command,
            IEnumerable<ApprenticeFeedbackTarget> feedbackApiResponse,
            GetApprenticeLearnerResponse learnerResponse,
            ApiResponse<ApprenticeFeedbackTarget> apiResponse)
        {
            // Arrange
            // Set up Targets
            _mockFeedbackApiClient.Setup(c => c.GetAll<ApprenticeFeedbackTarget>(
                It.Is<GetAllApprenticeFeedbackTargetsRequest>(x => x.ApprenticeId == command.ApprenticeId))).ReturnsAsync(feedbackApiResponse);

            // Set up Learners
            var learnerApiResponse = new ApiResponse<GetApprenticeLearnerResponse>(learnerResponse, HttpStatusCode.OK, string.Empty);
            foreach (var target in feedbackApiResponse)
            {
                _mockAssessorApiClient.Setup(s => s.GetWithResponseCode<GetApprenticeLearnerResponse>(
                    It.Is<GetApprenticeLearnerRequest>(s => s.ApprenticeCommitmentsId == target.ApprenticeshipId)))
                    .ReturnsAsync(learnerApiResponse);
            }

            // Don't care about update response, just that values are returned.
            var updateRequests = new List<UpdateApprenticeFeedbackTargetRequest>();
            _mockFeedbackApiClient.Setup(c => c.PostWithResponseCode<ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest, bool>((request, includeResponse) => updateRequests.Add((UpdateApprenticeFeedbackTargetRequest)request))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            foreach (var updateRequest in updateRequests)
            {
                var data = (UpdateApprenticeFeedbackTargetRequestData)updateRequest.Data;
                var target = feedbackApiResponse.Single(s => s.Id == data.ApprenticeFeedbackTargetId);

                target.Should().NotBeNull();
                data.Learner.Should().BeEquivalentTo(new
                {
                    learnerResponse.Ukprn,
                    learnerResponse.ProviderName,
                    learnerResponse.LearnStartDate,
                    learnerResponse.PlannedEndDate,
                    learnerResponse.StandardCode,
                    learnerResponse.StandardUId,
                    learnerResponse.StandardReference,
                    learnerResponse.StandardName,
                    learnerResponse.CompletionStatus,
                    learnerResponse.ApprovalsStopDate,
                    learnerResponse.ApprovalsPauseDate,
                    learnerResponse.LearnActEndDate,
                    learnerResponse.EstimatedEndDate

                });
            }

            result.Should().BeOfType<UpdateApprenticeFeedbackTargetResponse>();
        }
    }
}