using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;
using SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.Extensions;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Recruit.UnitTests.Application.VacancyReview.Commands;

public class WhenHandlingUpsertVacancyReviewCommand
{
    [Test, MoqAutoData]
                && c.Tokens["VACcode"] == command.VacancyReview.VacancyReference.ToString()
                && c.Tokens["location"] == "Recruiting nationally"
            )
        ), Times.Once);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Submitted_Vacancy_Sends_Immediate_Emails(
        UpsertVacancyReviewCommand command,
        PostCreateVacancyNotificationsResponse response,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] UpsertVacancyReviewCommandHandler sut)
    {
        // arrange
        command.VacancyReview.Status = "New";
        List<SendEmailCommand> sentEmails = [];
        var expectedEmails = response.Select(x => new SendEmailCommand(x.TemplateId.ToString(), x.RecipientAddress, x.Tokens));

        apiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutCreateVacancyReviewRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.OK, null));
        
        PostCreateVacancyNotificationsRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(It.IsAny<PostCreateVacancyNotificationsRequest>(), It.IsAny<bool>()))
            .Callback<IPostApiRequest, bool>((x, _) => capturedRequest = x as PostCreateVacancyNotificationsRequest)
            .ReturnsAsync(new ApiResponse<PostCreateVacancyNotificationsResponse>(response, HttpStatusCode.OK, null));
        
        notificationService
            .Setup(x => x.Send(It.IsAny<SendEmailCommand>()))
            .Callback<SendEmailCommand>(x => sentEmails.Add(x));

        // act
        await sut.Handle(command, CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.PostUrl.Should().Be($"api/vacancies/{command.VacancyReview.VacancyId}/create-notifications");
        capturedRequest.Data.Should().BeNull();
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Exactly(response.Count));
        sentEmails.Should().BeEquivalentTo(expectedEmails);
        
        apiClient.Verify(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<GetProviderRecruitUserNotificationPreferencesApiRequest>()), Times.Never);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Submitted_Emails_Does_Not_Return_Successfully_Then_No_Emails_Are_Sent(
        UpsertVacancyReviewCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<ILogger<UpsertVacancyReviewCommandHandler>> logger,
        [Greedy] UpsertVacancyReviewCommandHandler sut)
    {
        // arrange
        command.VacancyReview.Status = "New";

        apiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutCreateVacancyReviewRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.OK, null));
        
        apiClient
            .Setup(x => x.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(It.IsAny<PostCreateVacancyNotificationsRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<PostCreateVacancyNotificationsResponse>(null!, HttpStatusCode.BadRequest, null));

        // act
        await sut.Handle(command, CancellationToken.None);

        // assert
        logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        apiClient.Verify(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<GetProviderRecruitUserNotificationPreferencesApiRequest>()), Times.Never);
    }
}