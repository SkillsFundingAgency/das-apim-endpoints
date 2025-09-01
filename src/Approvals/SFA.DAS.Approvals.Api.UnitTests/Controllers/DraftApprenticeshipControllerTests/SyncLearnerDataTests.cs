using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    public class SyncLearnerDataTests
    {
        [Test, MoqAutoData]
        public async Task SyncLearnerData_WhenSuccessful_ReturnsOkResult(
            [Frozen] Mock<IMediator> mediator,
            SyncLearnerDataCommandResult commandResult,
            [Greedy] DraftApprenticeshipController controller)
        {
            // Arrange
            commandResult.Success = true;
            commandResult.Message = "Test success message";
            mediator.Setup(x => x.Send(It.IsAny<SyncLearnerDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResult);

            // Act
            var result = await controller.SyncLearnerData(1, 2, 3);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<SyncLearnerDataResponse>();
            var response = okResult.Value as SyncLearnerDataResponse;
            response.Success.Should().BeTrue();
            response.Message.Should().Be(commandResult.Message);
        }

        [Test, MoqAutoData]
        public async Task SyncLearnerData_WhenFailed_ReturnsOkResultWithFailure(
            [Frozen] Mock<IMediator> mediator,
            SyncLearnerDataCommandResult commandResult,
            [Greedy] DraftApprenticeshipController controller)
        {
            // Arrange
            commandResult.Success = false;
            commandResult.Message = "Test failure message";
            mediator.Setup(x => x.Send(It.IsAny<SyncLearnerDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResult);

            // Act
            var result = await controller.SyncLearnerData(1, 2, 3);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<SyncLearnerDataResponse>();
            var response = okResult.Value as SyncLearnerDataResponse;
            response.Success.Should().BeFalse();
            response.Message.Should().Be(commandResult.Message);
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
            SyncLearnerDataCommandResult commandResult,
            [Greedy] DraftApprenticeshipController controller)
        {
            // Arrange
            const long providerId = 123;
            const long cohortId = 456;
            const long draftApprenticeshipId = 789;

            mediator.Setup(x => x.Send(It.IsAny<SyncLearnerDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResult);

            // Act
            await controller.SyncLearnerData(providerId, cohortId, draftApprenticeshipId);

            // Assert
            mediator.Verify(x => x.Send(It.Is<SyncLearnerDataCommand>(c =>
                    c.ProviderId == providerId &&
                    c.CohortId == cohortId &&
                    c.DraftApprenticeshipId == draftApprenticeshipId),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}