using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.Handlers;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.UnitTests.Handlers;
[TestFixture]
internal class WhenHandlingSharedApplicationReviewedEventHandler
{
    [Test, MoqAutoData]
    public async Task Nothing_Happens_When_The_Vacancy_Is_Not_Found(
        SharedApplicationReviewedEvent @event,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] SharedApplicationReviewedEventHandler sut)
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
    public async Task Nothing_Happens_When_The_Vacancy_Is_Not_Of_The_Correct_Type(
        SharedApplicationReviewedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] SharedApplicationReviewedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.OwnerType = OwnerType.Employer;
        GetVacancyByIdQuery? capturedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetVacancyByIdQueryResult>, CancellationToken>((q, _) => capturedQuery = q as GetVacancyByIdQuery)
            .ReturnsAsync(vacancyResponse);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedQuery.Should().NotBeNull();
        capturedQuery!.Id.Should().Be(@event.VacancyId);
        apiClient.Verify(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()), Times.Never);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Nothing_Happens_When_The_Vacancy_Is_Not_In_The_Correct_State(
        SharedApplicationReviewedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] SharedApplicationReviewedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.OwnerType = OwnerType.Employer;
        GetVacancyByIdQuery? capturedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetVacancyByIdQueryResult>, CancellationToken>((q, _) => capturedQuery = q as GetVacancyByIdQuery)
            .ReturnsAsync(vacancyResponse);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedQuery.Should().NotBeNull();
        capturedQuery!.Id.Should().Be(@event.VacancyId);
        apiClient.Verify(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()), Times.Never);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task No_Emails_Sent_When_No_Users_Found_Who_Should_Receive_Email(
        SharedApplicationReviewedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] SharedApplicationReviewedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.OwnerType = OwnerType.Provider;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        GetProviderRecruitUserNotificationPreferencesApiRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .Callback<IGetAllApiRequest>(x => capturedRequest = x as GetProviderRecruitUserNotificationPreferencesApiRequest)
            .ReturnsAsync([
                new RecruitUserApiResponse
                {
                    NotificationPreferences = new NotificationPreferences
                    {
                        EventPreferences = [
                            new EventPreference
                            {
                                Event = NotificationTypes.SharedApplicationReviewedByEmployer,
                                Scope = NotificationScope.UserSubmittedVacancies,
                            }
                        ]
                    }
                }]);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetAllUrl.Should().Contain($"ukprn/{vacancyResponse.Vacancy.TrainingProvider!.Ukprn!.Value}?notificationType=SharedApplicationReviewedByEmployer");
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }


    [Test, MoqAutoData]
    public async Task Emails_Sent_When_Users_Found_Who_Should_Receive_Email(
        SharedApplicationReviewedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] SharedApplicationReviewedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.OwnerType = OwnerType.Provider;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        GetProviderRecruitUserNotificationPreferencesApiRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .Callback<IGetAllApiRequest>(x => capturedRequest = x as GetProviderRecruitUserNotificationPreferencesApiRequest)
            .ReturnsAsync([
                new RecruitUserApiResponse
                {
                    NotificationPreferences = new NotificationPreferences
                    {
                        EventPreferences = [
                            new EventPreference
                            {
                                Event = NotificationTypes.SharedApplicationReviewedByEmployer,
                                Scope = NotificationScope.OrganisationVacancies,
                            }
                        ]
                    }
                }]);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetAllUrl.Should().Contain($"ukprn/{vacancyResponse.Vacancy.TrainingProvider!.Ukprn!.Value}?notificationType=SharedApplicationReviewedByEmployer");
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Email_Sent_To_Provider_Submitter(
        SharedApplicationReviewedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Greedy] SharedApplicationReviewedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.OwnerType = OwnerType.Provider;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        GetProviderRecruitUserNotificationPreferencesApiRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .Callback<IGetAllApiRequest>(x => capturedRequest = x as GetProviderRecruitUserNotificationPreferencesApiRequest)
            .ReturnsAsync([
                new RecruitUserApiResponse
                {
                    Id = vacancyResponse.Vacancy.ReviewRequestedByUserId!.Value,
                    NotificationPreferences = new NotificationPreferences
                    {
                        EventPreferences = [
                            new EventPreference
                            {
                                Event = NotificationTypes.SharedApplicationReviewedByEmployer,
                                Scope = NotificationScope.OrganisationVacancies,
                            }
                        ]
                    }
                }]);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetAllUrl.Should().Contain($"ukprn/{vacancyResponse.Vacancy.TrainingProvider!.Ukprn!.Value}?notificationType=SharedApplicationReviewedByEmployer");
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }
}
