using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Application.UnitTests.DraftApprenticeships.Commands.SyncLearnerData
{
    [TestFixture]
    public class SyncLearnerDataCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_WhenDraftApprenticeshipNotFound_ReturnsFailureResult(
            SyncLearnerDataCommand command,
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler)
        {
            // Arrange
            commitmentsApiClient
                .Setup(x => x.GetDraftApprenticeship(command.CohortId, command.DraftApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetDraftApprenticeshipResponse)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Draft apprenticeship not found.");
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenNoLearnerDataChanges_ReturnsSuccessResult(
            SyncLearnerDataCommand command,
            GetDraftApprenticeshipResponse draftApprenticeship,
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler)
        {
            // Arrange
            draftApprenticeship.HasLearnerDataChanges = false;
            commitmentsApiClient
                .Setup(x => x.GetDraftApprenticeship(command.CohortId, command.DraftApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(draftApprenticeship);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("No learner data changes to sync.");
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenHasLearnerDataChanges_ReturnsSuccessResult(
            SyncLearnerDataCommand command,
            GetDraftApprenticeshipResponse draftApprenticeship,
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler)
        {
            // Arrange
            draftApprenticeship.HasLearnerDataChanges = true;
            commitmentsApiClient
                .Setup(x => x.GetDraftApprenticeship(command.CohortId, command.DraftApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(draftApprenticeship);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Learner data has been successfully updated.");
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailureResult(
            SyncLearnerDataCommand command,
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler)
        {
            // Arrange
            commitmentsApiClient
                .Setup(x => x.GetDraftApprenticeship(command.CohortId, command.DraftApprenticeshipId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("An error occurred while syncing learner data.");
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenCalled_CallsCommitmentsApiClientWithCorrectParameters(
            SyncLearnerDataCommand command,
            GetDraftApprenticeshipResponse draftApprenticeship,
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler)
        {
            // Arrange
            draftApprenticeship.HasLearnerDataChanges = false;
            commitmentsApiClient
                .Setup(x => x.GetDraftApprenticeship(command.CohortId, command.DraftApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(draftApprenticeship);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            commitmentsApiClient.Verify(x => x.GetDraftApprenticeship(command.CohortId, command.DraftApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

