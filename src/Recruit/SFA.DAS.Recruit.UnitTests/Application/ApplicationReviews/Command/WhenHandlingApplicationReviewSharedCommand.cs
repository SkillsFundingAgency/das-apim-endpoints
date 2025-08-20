using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.ApplicationReviewShared;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Command;

[TestFixture]
internal class WhenHandlingApplicationReviewSharedCommand
{
    [Test, MoqAutoData]
    public async Task Handle_Should_Send_Email_With_Expected_Parameters(
        ApplicationReviewSharedCommand command,
        [Frozen] EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] ApplicationReviewSharedCommandHandler handler)
    {
        // Arrange
        emailEnvironmentHelper = new EmailEnvironmentHelper("local");
        SendEmailCommand? capturedCommand = null;

        notificationService
            .Setup(s => s.Send(It.IsAny<SendEmailCommand>()))
            .Callback<SendEmailCommand>(cmd => capturedCommand = cmd)
            .Returns(Task.CompletedTask);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        notificationService.Verify(s => s.Send(It.IsAny<SendEmailCommand>()), Times.Once);

        capturedCommand.Should().NotBeNull();
        capturedCommand.TemplateId.Should().Be(emailEnvironmentHelper.ApplicationReviewSharedEmailTemplatedId);
        capturedCommand.RecipientsAddress.Should().Be(command.RecipientEmail);

        capturedCommand.Tokens.Should().ContainKey("firstName").WhoseValue.Should().Be(command.FirstName);
        capturedCommand.Tokens.Should().ContainKey("trainingProvider").WhoseValue.Should().Be(command.TrainingProvider);
        capturedCommand.Tokens.Should().ContainKey("vacancyReference").WhoseValue.Should().Be(command.VacancyReference.ToString());

        capturedCommand.Tokens.Should().ContainKey("applicationUrl")
            .WhoseValue.Should().EndWith($"accounts/{command.HashAccountId}/vacancies/{command.VacancyId}/applications/{command.ApplicationId}/?vacancySharedByProvider=True");
    }
}
