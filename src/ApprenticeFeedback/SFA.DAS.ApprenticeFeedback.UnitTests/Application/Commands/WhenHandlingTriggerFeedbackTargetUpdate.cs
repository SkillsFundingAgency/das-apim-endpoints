using FluentAssertions;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.ApprenticeFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Commands
{
    public class WhenHandlingTriggerFeedbackTargetUpdate
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockFeedbackApiClient;
        private Mock<IApprenticeshipDetailsService> _mockApprenticeshipDetailsService;
        private TriggerFeedbackTargetUpdateCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockFeedbackApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _mockApprenticeshipDetailsService = new Mock<IApprenticeshipDetailsService>();

            _handler = new TriggerFeedbackTargetUpdateCommandHandler(
                _mockFeedbackApiClient.Object, _mockApprenticeshipDetailsService.Object, Mock.Of<ILogger<TriggerFeedbackTargetUpdateCommandHandler>>());
        }

        [Test, MoqAutoData]
        public async Task Then_PostToUpdateFeedbackTargetIsSent_WhenLearnerDataIsAvailable(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = new LearnerData(), MyApprenticeshipData = null });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.Is<UpdateApprenticeFeedbackTargetRequest>(x => x.PostUrl == "api/apprenticefeedbacktarget/update"), true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ResponseIsTrue_WhenLearnerDataIsAvailable(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = new LearnerData(), MyApprenticeshipData = null });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(new TriggerFeedbackTargetUpdateResponse { Success = true });
        }

        [Test, MoqAutoData]
        public async Task Then_PostToUpdateFeedbackTargetIsSent_WhenMyApprenticeshipDataIsAvailable(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = null, MyApprenticeshipData = new MyApprenticeshipData() });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.Is<UpdateApprenticeFeedbackTargetRequest>(x => x.PostUrl == "api/apprenticefeedbacktarget/update"), true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ResponseIsTrue_WhenMyApprenticeshipDataIsAvailable(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = null, MyApprenticeshipData = new MyApprenticeshipData() });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(new TriggerFeedbackTargetUpdateResponse { Success = true });
        }

        [Test, MoqAutoData]
        public async Task Then_PostToUpdateFeedbackTargetIsSent_WhenLearnerDataAndMyApprenticeshipDataIsAvailable(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = new LearnerData(), MyApprenticeshipData = new MyApprenticeshipData() });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.Is<UpdateApprenticeFeedbackTargetRequest>(x => x.PostUrl == "api/apprenticefeedbacktarget/update"), true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ResponseIsTrue_WhenLearnerDataAndMyApprenticeshipDataIsAvailable(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = new LearnerData(), MyApprenticeshipData = new MyApprenticeshipData() });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(new TriggerFeedbackTargetUpdateResponse { Success = true });
        }

        [Test, MoqAutoData]
        public async Task Then_ResponseIsFalse_WhenUpdateApprenticeFeedbackTargetIsSent_AndFails(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(null, HttpStatusCode.InternalServerError, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = new LearnerData(), MyApprenticeshipData = new MyApprenticeshipData() });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(new TriggerFeedbackTargetUpdateResponse 
            { 
                Success = false,
                Message = $"Error updating the apprentice feedback target with ApprenticeFeedbackTargetId: {command.ApprenticeFeedbackTargetId}, Content: "
            });
        }

        [Test, MoqAutoData]
        public async Task Then_PostToUpdateFeedbackTargetDeferIsSent_WhenNeitherLearnerDataOrMyApprenticeshipDataIsAvailable(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetDeferRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetDeferRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = null, MyApprenticeshipData = null });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<UpdateApprenticeFeedbackTargetDeferRequestData, ApprenticeFeedbackTarget>(It.Is<UpdateApprenticeFeedbackTargetDeferRequest>(x => x.PostUrl == "api/apprenticefeedbacktarget/deferUpdate"), true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ResponseIsTrue_WhenNeitherLearnerDataOrMyApprenticeshipDataIsAvailable(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetDeferRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetDeferRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = null, MyApprenticeshipData = null });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(new TriggerFeedbackTargetUpdateResponse { Success = true });
        }

        [Test, MoqAutoData]
        public async Task Then_ResponseIsFalse_WhenUpdateFeedbackTargetDeferIsSent_AndFails(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetDeferRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetDeferRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(null, HttpStatusCode.InternalServerError, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(new ApprenticeshipDetails { LearnerData = null, MyApprenticeshipData = null });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(new TriggerFeedbackTargetUpdateResponse 
            { 
                Success = false, 
                Message = $"Error deferring update to the apprentice feedback target with ApprenticeFeedbackTargetId: {command.ApprenticeFeedbackTargetId}, Content: " 
            });
        }
    }
}
