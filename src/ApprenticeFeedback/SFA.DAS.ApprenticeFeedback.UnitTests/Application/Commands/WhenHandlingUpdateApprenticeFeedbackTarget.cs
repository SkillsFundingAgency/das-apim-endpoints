using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Commands
{
    public class WhenHandlingUpdateApprenticeFeedbackTarget
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockFeedbackApiClient;
        private Mock<IApprenticeshipDetailsService> _mockApprenticeshipDetailsService;
        private UpdateApprenticeFeedbackTargetCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockFeedbackApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _mockApprenticeshipDetailsService = new Mock<IApprenticeshipDetailsService>();

            _handler = new UpdateApprenticeFeedbackTargetCommandHandler(
                _mockFeedbackApiClient.Object, _mockApprenticeshipDetailsService.Object, Mock.Of<ILogger<UpdateApprenticeFeedbackTargetCommandHandler>>());
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
            LearnerData learnerData,
            MyApprenticeshipData myApprenticeshipData,
            ApprenticeFeedbackTarget apprenticeFeedbackTarget)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(c => c.GetAll<ApprenticeFeedbackTarget>(It.Is<GetAllApprenticeFeedbackTargetsRequest>(x => x.ApprenticeId == command.ApprenticeId)))
                .ReturnsAsync(feedbackApiResponse);

            // Don't care about responses here, just to pass the test.
            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync((learnerData, myApprenticeshipData));

            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(apprenticeFeedbackTarget, HttpStatusCode.OK, string.Empty));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            foreach (var target in feedbackApiResponse)
            {
                _mockApprenticeshipDetailsService.Verify(s => s.Get(It.Is<Guid>(s => s == target.ApprenticeId), It.Is<long>(s => s == target.ApprenticeshipId)), Times.Once);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_PostForUpdateOfTarget_EqualsReturnedApprenticeFeedbackTargetsWithLearnerData(
            UpdateApprenticeFeedbackTargetCommand command,
            IEnumerable<ApprenticeFeedbackTarget> feedbackApiResponse,
            LearnerData learnerData,
            MyApprenticeshipData myApprenticeshipData,
            ApiResponse<ApprenticeFeedbackTarget> apiResponse)
        {
            // Arrange
            // Set up Targets
            _mockFeedbackApiClient.Setup(c => c.GetAll<ApprenticeFeedbackTarget>(
                It.Is<GetAllApprenticeFeedbackTargetsRequest>(x => x.ApprenticeId == command.ApprenticeId))).ReturnsAsync(feedbackApiResponse);

            // Set up Learners
            foreach (var target in feedbackApiResponse)
            {
                _mockApprenticeshipDetailsService
                    .Setup(s => s.Get(It.Is<Guid>(s => s == target.ApprenticeId), It.Is<long>(s => s == target.ApprenticeshipId)))
                    .ReturnsAsync((learnerData, myApprenticeshipData));
            }

            // Don't care about update response, just that values are returned.
            var updateRequests = new List<UpdateApprenticeFeedbackTargetRequest>();
            _mockFeedbackApiClient
                .Setup(c => c.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest<UpdateApprenticeFeedbackTargetRequestData>, bool>((request, includeResponse) => updateRequests.Add((UpdateApprenticeFeedbackTargetRequest)request))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            foreach (var updateRequest in updateRequests)
            {
                var data = updateRequest.Data;
                var target = feedbackApiResponse.Single(s => s.Id == data.ApprenticeFeedbackTargetId);

                target.Should().NotBeNull();
                data.Learner.Should().BeEquivalentTo(new
                {
                    learnerData.Ukprn,
                    learnerData.ProviderName,
                    learnerData.LearnStartDate,
                    learnerData.PlannedEndDate,
                    learnerData.StandardCode,
                    learnerData.StandardUId,
                    learnerData.StandardReference,
                    learnerData.StandardName,
                    learnerData.CompletionStatus,
                    learnerData.ApprovalsStopDate,
                    learnerData.ApprovalsPauseDate,
                    learnerData.LearnActEndDate,
                    learnerData.EstimatedEndDate
                });
                data.MyApprenticeship.Should().BeEquivalentTo(new
                {
                    myApprenticeshipData.TrainingProviderId,
                    myApprenticeshipData.TrainingProviderName,
                    myApprenticeshipData.TrainingCode,
                    myApprenticeshipData.StandardUId,
                    myApprenticeshipData.StartDate,
                    myApprenticeshipData.EndDate
                });
            }

            result.Should().BeOfType<UpdateApprenticeFeedbackTargetResponse>();
        }
    }
}