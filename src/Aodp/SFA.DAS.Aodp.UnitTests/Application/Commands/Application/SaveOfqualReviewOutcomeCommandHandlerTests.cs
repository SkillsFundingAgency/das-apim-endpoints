using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Models;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.Aodp.Application.Tests.Commands.Application.Review;

[TestFixture]
public class SaveOfqualReviewOutcomeCommandHandlerOuterTests
{
    private static readonly Guid ApplicationReviewId = Guid.NewGuid();

    private const string OfqualUserEmail = "ofqual@test.com";
    private const string OfqualUserName = "Ofqual User";
    private const string OutcomeComments = "Looks good";

    private const string TemplateName = EmailTemplateNames.QFASTSubmittedApplicationChangedNotification;
    private const NotificationRecipientKind RecipientKind = NotificationRecipientKind.QfauMailbox;

    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClient = null!;
    private Mock<IEmailService> _emailService = null!;

    private SaveOfqualReviewOutcomeCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _apiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _emailService = new Mock<IEmailService>();

        _handler = new SaveOfqualReviewOutcomeCommandHandler(
            _apiClient.Object,
            _emailService.Object);
    }

    [Test]
    public async Task Handle_ValidResponse_SendsEmail_AndReturnsSuccess()
    {
        // Arrange
        var notifications = new List<NotificationDefinition>
        {
            new()
            {
                TemplateName = TemplateName,
                RecipientKind = RecipientKind
            }
        };

        var innerResponse = new SaveOfqualReviewOutcomeCommandResponse
        {
            Notifications = notifications
        };

        var apiResponse = new ApiResponse<SaveOfqualReviewOutcomeCommandResponse>(
            innerResponse,
            HttpStatusCode.OK,
            string.Empty);

        _apiClient
            .Setup(c => c.PutWithResponseCode<SaveOfqualReviewOutcomeCommandResponse>(
                It.IsAny<SaveOfqualReviewOutcomeApiRequest>()))
            .ReturnsAsync(apiResponse);

        var request = new SaveOfqualReviewOutcomeCommand
        {
            ApplicationReviewId = ApplicationReviewId,
            Comments = OutcomeComments,
            SentByEmail = OfqualUserEmail,
            SentByName = OfqualUserName
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            _apiClient.Verify(c =>
                c.PutWithResponseCode<SaveOfqualReviewOutcomeCommandResponse>(
                    It.Is<SaveOfqualReviewOutcomeApiRequest>(r =>
                        r.PutUrl == $"/api/application-reviews/{ApplicationReviewId}/ofqual-outcome"
                        && r.Data == request)),
                Times.Once);

            _emailService.Verify(s => s.SendAsync(
                It.Is<IReadOnlyCollection<NotificationDefinition>>(n =>
                    n.Count == 1 &&
                    n.First().TemplateName == TemplateName &&
                    n.First().RecipientKind == RecipientKind
                ),
                default), Times.Once);
        });
    }

    [Test]
    public async Task Handle_NoNotifications_EmptyPayloadToEmailService_StillReturnsSuccess()
    {
        // Arrange
        var innerResponse = new SaveOfqualReviewOutcomeCommandResponse
        {
            Notifications = new List<NotificationDefinition>()
        };

        var apiResponse = new ApiResponse<SaveOfqualReviewOutcomeCommandResponse>(
            innerResponse,
            HttpStatusCode.OK,
            string.Empty);

        _apiClient
            .Setup(c => c.PutWithResponseCode<SaveOfqualReviewOutcomeCommandResponse>(
                It.IsAny<SaveOfqualReviewOutcomeApiRequest>()))
            .ReturnsAsync(apiResponse);

        var request = new SaveOfqualReviewOutcomeCommand
        {
            ApplicationReviewId = ApplicationReviewId,
            Comments = OutcomeComments,
            SentByEmail = OfqualUserEmail,
            SentByName = OfqualUserName
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            _emailService.Verify(
                s => s.SendAsync(
                    It.Is<IReadOnlyCollection<NotificationDefinition>>(n => n != null && !n.Any()),
                    default),
                Times.Once);
        });
    }

    [Test]
    public async Task Handle_ApiClientThrows_ReturnsError_DoesNotSendEmail()
    {
        // Arrange
        _apiClient
            .Setup(c => c.PutWithResponseCode<SaveOfqualReviewOutcomeCommandResponse>(
                It.IsAny<SaveOfqualReviewOutcomeApiRequest>()))
            .ThrowsAsync(new InvalidOperationException("api exception"));

        var request = new SaveOfqualReviewOutcomeCommand
        {
            ApplicationReviewId = ApplicationReviewId,
            Comments = OutcomeComments,
            SentByEmail = OfqualUserEmail,
            SentByName = OfqualUserName
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("api exception"));

            _emailService.Verify(
                s => s.SendAsync(It.IsAny<IReadOnlyCollection<NotificationDefinition>>(), default),
                Times.Never);
        });
    }
}
