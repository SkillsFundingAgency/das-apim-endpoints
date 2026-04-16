using System;
using System.Net;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.Handlers;
using SFA.DAS.Recruit.InnerApi.Models;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Handlers;

public class WhenHandlingVacancySubmittedEventHandler
{
    [Test, MoqAutoData]
    public async Task Then_The_Emails_Are_Returned_And_Passed_To_The_Notification_Service(
        VacancySubmittedEvent @event,
        NotificationEmailDto email,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] VacancySubmittedEventHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<PostCreateVacancyNotificationsResponse>([email], HttpStatusCode.OK, null);
        PostCreateVacancyNotificationsRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(
                It.IsAny<PostCreateVacancyNotificationsRequest>(), true))
            .Callback<IPostApiRequest, bool>((x, _) => capturedRequest = x as PostCreateVacancyNotificationsRequest)
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
        capturedRequest!.PostUrl.Should().Be($"api/vacancies/{@event.VacancyId}/create-notifications");
        
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
        capturedCommand.Should().NotBeNull();
        capturedCommand!.TemplateId.Should().Be(email.TemplateId.ToString());
        capturedCommand.Should().BeEquivalentTo(email, opt => opt.ExcludingMissingMembers().Excluding(x => x.TemplateId));
    }
    
    [Test, MoqAutoData]
    public async Task Then_No_Emails_Are_Sent_When_The_Remote_Call_Fails(
        VacancySubmittedEvent @event,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<ILogger<VacancySubmittedEventHandler>> logger,
        [Greedy] VacancySubmittedEventHandler sut)
    {
        // arrange
        var apiResponse = new ApiResponse<PostCreateVacancyNotificationsResponse>(null!, HttpStatusCode.BadRequest, "foo");
        apiClient
            .Setup(x => x.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(
                It.IsAny<PostCreateVacancyNotificationsRequest>(), true))
            .ReturnsAsync(apiResponse);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }
}