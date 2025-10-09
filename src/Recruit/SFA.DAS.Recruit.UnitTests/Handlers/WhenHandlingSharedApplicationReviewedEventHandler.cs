using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.Handlers;
using SFA.DAS.Recruit.InnerApi.Models;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Handlers;

public class WhenHandlingSharedApplicationReviewedEventHandler
{
    [Test, MoqAutoData]
    public async Task Then_The_Emails_Are_Returned_And_Passed_To_The_Notification_Service(
        SharedApplicationReviewedEvent @event,
        NotificationEmailDto email,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] SharedApplicationReviewedEventHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<List<NotificationEmailDto>>([email], HttpStatusCode.OK, null);
        PostCreateApplicationReviewNotificationsRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.PostWithResponseCode<List<NotificationEmailDto>>(
                It.IsAny<PostCreateApplicationReviewNotificationsRequest>(), true))
            .Callback<IPostApiRequest, bool>((x, _) => capturedRequest = x as PostCreateApplicationReviewNotificationsRequest)
            .ReturnsAsync(apiResponse);

        SendEmailCommand? capturedCommand = null;
        notificationService
            .Setup(x => x.Send(It.IsAny<SendEmailCommand>()))
            .Callback<SendEmailCommand>(x => capturedCommand = x)
            .Returns(Task.CompletedTask);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.PostUrl.Should().Be($"api/applicationreviews/{@event.ApplicationReviewId}/create-notifications");
        
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
        capturedCommand.Should().NotBeNull();
        capturedCommand!.TemplateId.Should().Be(email.TemplateId.ToString());
        capturedCommand.Should().BeEquivalentTo(email, opt => opt.ExcludingMissingMembers().Excluding(x => x.TemplateId));
    }
    
    [Test, MoqAutoData]
    public async Task Then_No_Emails_Are_Sent_When_The_Remote_Call_Fails(
        SharedApplicationReviewedEvent @event,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<ILogger<SharedApplicationReviewedEventHandler>> logger,
        [Greedy] SharedApplicationReviewedEventHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<List<NotificationEmailDto>>(null!, HttpStatusCode.BadRequest, "foo");
        apiClient
            .Setup(x => x.PostWithResponseCode<List<NotificationEmailDto>>(
                It.IsAny<PostCreateApplicationReviewNotificationsRequest>(), true))
            .ReturnsAsync(apiResponse);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }
}
