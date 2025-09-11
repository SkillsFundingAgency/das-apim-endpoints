using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.Handlers;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Handlers;

public class WhenHandlingApplicationSubmittedEventHandler
{
    [Test, MoqAutoData]
    public async Task Nothing_Happens_When_The_Vacancy_Is_Not_Found(
        ApplicationSubmittedEvent @event,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] ApplicationSubmittedEventHandler sut)
    {
        // arrange
        GetVacancyByIdQuery? capturedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetVacancyByIdQueryResult>, CancellationToken>((q, _) => capturedQuery = q as GetVacancyByIdQuery)
            .ReturnsAsync(GetVacancyByIdQueryResult.None);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedQuery.Should().NotBeNull();
        capturedQuery!.Id.Should().Be(@event.VacancyId);
        apiClient.Verify(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()), Times.Never);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Nothing_Happens_When_The_Vacancy_Has_No_Employer_AccountId(
        ApplicationSubmittedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] ApplicationSubmittedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.AccountId = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        apiClient.Verify(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()), Times.Never);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task No_Emails_Sent_When_No_Users_Found_Who_Should_Receive_Email(
        ApplicationSubmittedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] ApplicationSubmittedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.AccountId = 123;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        GetEmployerRecruitUserNotificationPreferencesApiRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .Callback<IGetAllApiRequest>(x => capturedRequest = x as GetEmployerRecruitUserNotificationPreferencesApiRequest)
            .ReturnsAsync([
                new RecruitUserApiResponse
                {
                    NotificationPreferences = new NotificationPreferences
                    {
                        EventPreferences =
                        [
                            new EventPreference
                            {
                                Event = NotificationTypes.ApplicationSubmitted,
                                Frequency = NotificationFrequency.Never,
                                Scope = NotificationScope.OrganisationVacancies
                            }
                        ]
                    }
                }
            ]);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetAllUrl.Should().Contain(vacancyResponse.Vacancy.AccountId.ToString());
        capturedRequest!.GetAllUrl.Should().Contain("notificationType=ApplicationSubmitted");
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Emails_Sent_When_Users_With_Organisation_Preferences_Found(
        ApplicationSubmittedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] ApplicationSubmittedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.AccountId = 123;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        apiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .ReturnsAsync([
                new RecruitUserApiResponse
                {
                    NotificationPreferences = new NotificationPreferences
                    {
                        EventPreferences =
                        [
                            new EventPreference
                            {
                                Event = NotificationTypes.ApplicationSubmitted,
                                Frequency = NotificationFrequency.Immediately,
                                Scope = NotificationScope.OrganisationVacancies
                            }
                        ]
                    }
                }
            ]);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Email_Sent_To_User_Who_Submitted_The_Vacancy(
        ApplicationSubmittedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] ApplicationSubmittedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.AccountId = 123;
        var submittingUserId = vacancyResponse.Vacancy.SubmittedByUserId ?? Guid.NewGuid();
        vacancyResponse.Vacancy.SubmittedByUserId = submittingUserId;

        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        apiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .ReturnsAsync([
                new RecruitUserApiResponse
                {
                    Id = submittingUserId,
                    NotificationPreferences = new NotificationPreferences
                    {
                        EventPreferences =
                        [
                            new EventPreference
                            {
                                Event = NotificationTypes.ApplicationSubmitted,
                                Frequency = NotificationFrequency.Immediately,
                                Scope = NotificationScope.UserSubmittedVacancies
                            }
                        ]
                    }
                }
            ]);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }
}
