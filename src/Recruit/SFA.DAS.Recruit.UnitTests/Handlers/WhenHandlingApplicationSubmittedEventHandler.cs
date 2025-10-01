using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.Handlers;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using InnerResponses = SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Recruit.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

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
        apiClient.Verify(x => x.GetAll<InnerResponses.RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()), Times.Never);
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
        apiClient.Verify(x => x.GetAll<InnerResponses.RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()), Times.Never);
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
            .Setup(x => x.GetAll<InnerResponses.RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .Callback<IGetAllApiRequest>(x => capturedRequest = x as GetEmployerRecruitUserNotificationPreferencesApiRequest)
            .ReturnsAsync(new[]
            {
                new InnerResponses.RecruitUserApiResponse
                {
                    NotificationPreferences = new InnerResponses.NotificationPreferences
                    {
                        EventPreferences = new List<InnerResponses.EventPreference>
                        {
                            new InnerResponses.EventPreference
                            {
                                Event = NotificationTypes.ApplicationSubmitted,
                                Frequency = NotificationFrequency.Never,
                                Scope = NotificationScope.OrganisationVacancies
                            }
                        }
                    }
                }
            });

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
        [Frozen] EmailEnvironmentHelper emailHelper,
        [Greedy] ApplicationSubmittedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.AccountId = 123;
        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        var user = new InnerResponses.RecruitUserApiResponse
        {
            Email = "notify@example.com",
            Name = "Notify",
            NotificationPreferences = new InnerResponses.NotificationPreferences
            {
                EventPreferences = new List<InnerResponses.EventPreference>
                {
                    new() {
                        Event = NotificationTypes.ApplicationSubmitted,
                        Frequency = NotificationFrequency.Immediately,
                        Scope = NotificationScope.OrganisationVacancies
                    }
                }
            }
        };

        SendEmailCommand? capturedEmail = null;
        apiClient
            .Setup(x => x.GetAll<InnerResponses.RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .ReturnsAsync(new[] { user });

        notificationService
            .Setup(x => x.Send(It.IsAny<SendEmailCommand>()))
            .Callback<SendEmailCommand>(c => capturedEmail = c)
            .Returns(Task.CompletedTask);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedEmail.Should().NotBeNull();
        capturedEmail!.TemplateId.Should().Be(emailHelper.ApplicationSubmittedTemplateId);
        capturedEmail.RecipientsAddress.Should().Be(user.Email);

        var keys = capturedEmail.Tokens.Keys.ToList();
        keys.Should().ContainInOrder(new[] { "advertTitle", "firstName", "employerName", "manageAdvertURL", "notificationSettingsURL", "VACcode", "location" });

        capturedEmail.Tokens["advertTitle"].Should().Be(vacancyResponse.Vacancy.Title);
        capturedEmail.Tokens["firstName"].Should().Be(user.FirstName);
        capturedEmail.Tokens["employerName"].Should().Be(vacancyResponse.Vacancy.EmployerName);
        capturedEmail.Tokens["VACcode"].Should().Be(vacancyResponse.Vacancy.VacancyReference!.Value.ToString());
        capturedEmail.Tokens["location"].Should().NotBeNull();
        capturedEmail.Tokens["manageAdvertURL"].Should().Contain(vacancyResponse.Vacancy.Id.ToString());
        capturedEmail.Tokens["notificationSettingsURL"].Should().Contain(vacancyResponse.Vacancy.AccountId.ToString());
    }

    [Test, MoqAutoData]
    public async Task Email_Sent_To_User_Who_Submitted_The_Vacancy(
        ApplicationSubmittedEvent @event,
        GetVacancyByIdQueryResult vacancyResponse,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] EmailEnvironmentHelper emailHelper,
        [Greedy] ApplicationSubmittedEventHandler sut)
    {
        // arrange
        vacancyResponse.Vacancy.AccountId = 123;
        var submittingUserId = vacancyResponse.Vacancy.SubmittedByUserId ?? Guid.NewGuid();
        vacancyResponse.Vacancy.SubmittedByUserId = submittingUserId;

        mediator
            .Setup(x => x.Send(It.IsAny<GetVacancyByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancyResponse);

        var user = new InnerResponses.RecruitUserApiResponse
        {
            Id = submittingUserId,
            Email = "submitter@example.com",
            Name = "Submitter",
            NotificationPreferences = new InnerResponses.NotificationPreferences
            {
                EventPreferences = new List<InnerResponses.EventPreference>
                {
                    new() {
                        Event = NotificationTypes.ApplicationSubmitted,
                        Frequency = NotificationFrequency.Immediately,
                        Scope = NotificationScope.UserSubmittedVacancies
                    }
                }
            }
        };

        SendEmailCommand? capturedEmail = null;
        apiClient
            .Setup(x => x.GetAll<InnerResponses.RecruitUserApiResponse>(It.IsAny<IGetAllApiRequest>()))
            .ReturnsAsync(new[] { user });

        notificationService
            .Setup(x => x.Send(It.IsAny<SendEmailCommand>()))
            .Callback<SendEmailCommand>(c => capturedEmail = c)
            .Returns(Task.CompletedTask);

        // act
        await sut.Handle(@event, CancellationToken.None);

        // assert
        capturedEmail.Should().NotBeNull();
        capturedEmail!.TemplateId.Should().Be(emailHelper.ApplicationSubmittedTemplateId);
        capturedEmail.RecipientsAddress.Should().Be(user.Email);

        var keys = capturedEmail.Tokens.Keys.ToList();
        keys.Should().ContainInOrder(new[] { "advertTitle", "firstName", "employerName", "manageAdvertURL", "notificationSettingsURL", "VACcode", "location" });

        capturedEmail.Tokens["advertTitle"].Should().Be(vacancyResponse.Vacancy.Title);
        capturedEmail.Tokens["firstName"].Should().Be(user.FirstName);
        capturedEmail.Tokens["employerName"].Should().Be(vacancyResponse.Vacancy.EmployerName);
        capturedEmail.Tokens["VACcode"].Should().Be(vacancyResponse.Vacancy.VacancyReference!.Value.ToString());
        capturedEmail.Tokens["location"].Should().NotBeNull();
        capturedEmail.Tokens["manageAdvertURL"].Should().Contain(vacancyResponse.Vacancy.Id.ToString());
        capturedEmail.Tokens["notificationSettingsURL"].Should().Contain(vacancyResponse.Vacancy.AccountId.ToString());

        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }
}
