using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    public class SyncLearnerDataTests
    {
        [Test, MoqAutoData]
        public async Task SyncLearnerData_WhenSuccessful_ReturnsOkResult(
            [Frozen] Mock<IMediator> mediator,
            GetDraftApprenticeshipResponse updatedDraftApprenticeship,
            [Greedy] DraftApprenticeshipController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<SyncLearnerDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedDraftApprenticeship);

            // Act
            var result = await controller.SyncLearnerData(1, 2, 3);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<SyncLearnerDataResponse>();
            var response = okResult.Value as SyncLearnerDataResponse;
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Learner data has been successfully merged");
            response.UpdatedDraftApprenticeship.Should().Be(updatedDraftApprenticeship);
        }

        [Test, MoqAutoData]
        public async Task SyncLearnerData_WhenLearnerDataSyncException_ReturnsOkWithFailure(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DraftApprenticeshipController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<SyncLearnerDataCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new LearnerDataSyncException("Failed to retrieve learner data"));

            // Act
            var result = await controller.SyncLearnerData(1, 2, 3);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<SyncLearnerDataResponse>();
            var response = okResult.Value as SyncLearnerDataResponse;
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Failed to retrieve learner data");
        }

        [Test, MoqAutoData]
        public async Task SyncLearnerData_WhenExceptionOccurs_ReturnsBadRequest(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DraftApprenticeshipController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<SyncLearnerDataCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await controller.SyncLearnerData(1, 2, 3);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeOfType<SyncLearnerDataResponse>();
            var response = badRequestResult.Value as SyncLearnerDataResponse;
            response.Success.Should().BeFalse();
            response.Message.Should().Be("An error occurred while syncing learner data.");
        }

        [Test, MoqAutoData]
        public async Task SyncLearnerData_VerifiesCommandParameters(
            [Frozen] Mock<IMediator> mediator,
            GetDraftApprenticeshipResponse updatedDraftApprenticeship,
            [Greedy] DraftApprenticeshipController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<SyncLearnerDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedDraftApprenticeship);

            // Act
            await controller.SyncLearnerData(1, 2, 3);

            // Assert
            mediator.Verify(x => x.Send(It.Is<SyncLearnerDataCommand>(c => 
                c.ProviderId == 1 && 
                c.CohortId == 2 && 
                c.DraftApprenticeshipId == 3), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}