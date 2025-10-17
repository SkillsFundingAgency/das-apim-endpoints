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
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.VacancyReview.Commands;

public class WhenHandlingUpsertVacancyReviewCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Called_For_Providers(
        UpsertVacancyReviewCommand command,
        RecruitUserApiResponse userApiResponse1,
        RecruitUserApiResponse userApiResponse2,
        RecruitUserApiResponse userApiResponse3,
        [Frozen] EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        UpsertVacancyReviewCommandHandler handler)
    {
        userApiResponse1.Name = "firstName lastName";
        userApiResponse1.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Immediately,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse2.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.ProviderAttachedToVacancy,
                Frequency = NotificationFrequency.Immediately,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse3.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Daily,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        command.VacancyReview.ManualOutcome = "Approved";
        command.VacancyReview.OwnerType = "Provider";
        command.VacancyReview.EmployerLocationOption = AvailableWhere.AcrossEngland;
        var expectedPutRequest = new PutCreateVacancyReviewRequest(command.Id, command.VacancyReview);
        recruitApiClient.Setup(
                x => x.PutWithResponseCode<NullResponse>(
                    It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.Created, ""));
        var expectedGetUrl = new GetProviderRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.Ukprn);
        recruitApiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(
                It.Is<GetProviderRecruitUserNotificationPreferencesApiRequest>(c =>
                    c.GetAllUrl.Equals(expectedGetUrl.GetAllUrl)))).ReturnsAsync([userApiResponse1, userApiResponse3, userApiResponse2]);

        await handler.Handle(command, CancellationToken.None);

        recruitApiClient.Verify(
            x => x.PutWithResponseCode<NullResponse>(
                It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)), Times.Once);
        
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == userApiResponse1.Email
                && c.TemplateId == emailEnvironmentHelper.VacancyReviewApprovedProviderTemplateId
                && c.Tokens["advertTitle"] == command.VacancyReview.VacancyTitle
                && c.Tokens["firstName"] == "firstName"
                && c.Tokens["employerName"] == command.VacancyReview.EmployerName
                && c.Tokens["FindAnApprenticeshipAdvertURL"] == string.Format(emailEnvironmentHelper.LiveVacancyUrl,command.VacancyReview.VacancyReference.ToString())
                && c.Tokens["notificationSettingsURL"] == string.Format(emailEnvironmentHelper.NotificationsSettingsProviderUrl, command.VacancyReview.Ukprn)
                && c.Tokens["VACcode"] == command.VacancyReview.VacancyReference.ToString()
                && c.Tokens["location"] == "Recruiting nationally"
            )
        ), Times.Once);
        recruitApiClient
            .Verify(x => x.GetAll<RecruitUserApiResponse>(
                It.IsAny<GetEmployerRecruitUserNotificationPreferencesApiRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_The_OwnerType_Employer_Command_Is_Handled_And_Api_Called_For_Providers_Attached_To_Vacancy(
        GetVacancyByIdQueryResult vacancyResponse,
        GetStandardsListResponse standardResponse,
        UpsertVacancyReviewCommand command,
        RecruitUserApiResponse userApiResponse1,
        RecruitUserApiResponse userApiResponse2,
        RecruitUserApiResponse userApiResponse3,
        RecruitUserApiResponse userApiResponse4,
        [Frozen] EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICourseService> courseService,
        UpsertVacancyReviewCommandHandler handler)
    {
        vacancyResponse.Vacancy.ProgrammeId = "1";
        vacancyResponse.Vacancy.EmployerLocationOption = AvailableWhere.AcrossEngland;
        foreach (var standard in standardResponse.Standards)
        {
            standard.Level = 3;
            standard.Title = "Standard Title";
            standard.LarsCode = 1;
        }
        userApiResponse1.Name = "firstName lastName";
        userApiResponse1.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Immediately,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse2.Name = "firstName lastName";
        userApiResponse2.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.ProviderAttachedToVacancy,
                Frequency = NotificationFrequency.Immediately,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse3.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Daily,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse4.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Immediately,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        command.VacancyReview.ManualOutcome = "Approved";
        command.VacancyReview.OwnerType = "Employer";
        command.VacancyReview.EmployerLocationOption = AvailableWhere.AcrossEngland;
        var expectedPutRequest = new PutCreateVacancyReviewRequest(command.Id, command.VacancyReview);
        recruitApiClient.Setup(
                x => x.PutWithResponseCode<NullResponse>(
                    It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.Created, ""));
        var expectedGetUrl = new GetProviderRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.Ukprn);
        recruitApiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(
                It.Is<GetProviderRecruitUserNotificationPreferencesApiRequest>(c =>
                    c.GetAllUrl.Equals(expectedGetUrl.GetAllUrl)))).ReturnsAsync([userApiResponse2,userApiResponse4]);
        var expectedGetUrlEmployer = new GetEmployerRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.AccountId);
        recruitApiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(
                It.Is<GetEmployerRecruitUserNotificationPreferencesApiRequest>(c =>
                    c.GetAllUrl.Equals(expectedGetUrlEmployer.GetAllUrl)))).ReturnsAsync([userApiResponse1, userApiResponse3]);

        mediator.Setup(x => x.Send(It.Is<GetVacancyByIdQuery>(c => c.Id == command.VacancyReview.VacancyId), CancellationToken.None)).ReturnsAsync(vacancyResponse);
        courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>("ActiveStandards")).ReturnsAsync(standardResponse);

        await handler.Handle(command, CancellationToken.None);

        recruitApiClient.Verify(
            x => x.PutWithResponseCode<NullResponse>(
                It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)), Times.Once);

        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.RecipientsAddress == userApiResponse1.Email
                && c.TemplateId == emailEnvironmentHelper.VacancyReviewApprovedEmployerTemplateId
                && c.Tokens["advertTitle"] == command.VacancyReview.VacancyTitle
                && c.Tokens["firstName"] == "firstName"
                && c.Tokens["employerName"] == command.VacancyReview.EmployerName
                && c.Tokens["FindAnApprenticeshipAdvertURL"] == string.Format(emailEnvironmentHelper.LiveVacancyUrl, command.VacancyReview.VacancyReference.ToString())
                && c.Tokens["notificationSettingsURL"] == string.Format(emailEnvironmentHelper.NotificationsSettingsEmployerUrl, command.VacancyReview.HashedAccountId)
                && c.Tokens["VACcode"] == command.VacancyReview.VacancyReference.ToString()
                && c.Tokens["location"] == "Recruiting nationally"
            )
        ), Times.Once);

        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
           c.TemplateId == emailEnvironmentHelper.ProviderAddedToEmployerVacancy
           && c.RecipientsAddress == userApiResponse2.Email
                && c.Tokens["firstName"] == userApiResponse2.FirstName
                && c.Tokens["advertTitle"] == vacancyResponse.Vacancy.Title
                && c.Tokens["VACnumber"] == vacancyResponse.Vacancy.VacancyReference.ToString()
                && c.Tokens["employer"] == vacancyResponse.Vacancy.EmployerName
                && c.Tokens["submitterEmail"] == command.VacancyReview.SubmittedByUserEmail
                && c.Tokens["location"] == "Recruiting nationally"
                && c.Tokens["applicationUrl"] == string.Format(emailEnvironmentHelper.LiveVacancyUrl, vacancyResponse.Vacancy.VacancyReference)
                && c.Tokens["courseTitle"] == "Standard Title"
                && c.Tokens["positions"] == (vacancyResponse.Vacancy.NumberOfPositions > 1 ? $"{vacancyResponse.Vacancy.NumberOfPositions} apprentices" : "1 apprentice")
                && c.Tokens["startDate"] == vacancyResponse.Vacancy.StartDate.ToDayMonthYearString()
                && c.Tokens["duration"] == vacancyResponse.Vacancy.Wage.GetDuration()
                && c.Tokens["notificationSettingsURL"] == string.Format(emailEnvironmentHelper.NotificationsSettingsProviderUrl, command.VacancyReview.Ukprn)
            )
        ), Times.Once);
        notificationService.Verify(x=>x.Send(It.IsAny<SendEmailCommand>()), Times.Exactly(2));
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_Command_Is_Approving_An_Employer_Created_Vacancy_Review_Then_Notifications_Sent_For_Employers_With_Immediate_And_NotSet_Notification(
        UpsertVacancyReviewCommand command,
        RecruitUserApiResponse userApiResponse1,
        RecruitUserApiResponse userApiResponse2,
        RecruitUserApiResponse userApiResponse3,
        [Frozen] EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        UpsertVacancyReviewCommandHandler handler)
    {
        userApiResponse1.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Immediately,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse2.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.NotSet,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse3.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Daily,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        command.VacancyReview.ManualOutcome = "Approved";
        command.VacancyReview.OwnerType = "Employer";
        command.VacancyReview.EmployerLocationOption = AvailableWhere.AcrossEngland;
        var expectedPutRequest = new PutCreateVacancyReviewRequest(command.Id, command.VacancyReview);
        recruitApiClient.Setup(
                x => x.PutWithResponseCode<NullResponse>(
                    It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.Created, ""));
        var expectedGetUrl = new GetEmployerRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.AccountId);
        recruitApiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(
                It.Is<GetEmployerRecruitUserNotificationPreferencesApiRequest>(c =>
                    c.GetAllUrl.Equals(expectedGetUrl.GetAllUrl)))).ReturnsAsync([userApiResponse1, userApiResponse2, userApiResponse3]);

        await handler.Handle(command, CancellationToken.None);

        recruitApiClient.Verify(
            x => x.PutWithResponseCode<NullResponse>(
                It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)), Times.Once);
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == userApiResponse1.Email
                && c.TemplateId == emailEnvironmentHelper.VacancyReviewApprovedEmployerTemplateId
                && c.Tokens["advertTitle"] == command.VacancyReview.VacancyTitle
                && c.Tokens["firstName"] == userApiResponse1.FirstName
                && c.Tokens["employerName"] == command.VacancyReview.EmployerName
                && c.Tokens["FindAnApprenticeshipAdvertURL"] == string.Format(emailEnvironmentHelper.LiveVacancyUrl,command.VacancyReview.VacancyReference.ToString())
                && c.Tokens["notificationSettingsURL"] == string.Format(emailEnvironmentHelper.NotificationsSettingsEmployerUrl, command.VacancyReview.HashedAccountId)
                && c.Tokens["VACcode"] == command.VacancyReview.VacancyReference.ToString()
                && c.Tokens["location"] == "Recruiting nationally"
            )
        ), Times.Once);
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == userApiResponse2.Email
                && c.TemplateId == emailEnvironmentHelper.VacancyReviewApprovedEmployerTemplateId
                && c.Tokens["advertTitle"] == command.VacancyReview.VacancyTitle
                && c.Tokens["firstName"] == userApiResponse2.Name
                && c.Tokens["employerName"] == command.VacancyReview.EmployerName
                && c.Tokens["FindAnApprenticeshipAdvertURL"] == string.Format(emailEnvironmentHelper.LiveVacancyUrl,command.VacancyReview.VacancyReference.ToString())
                && c.Tokens["notificationSettingsURL"] == string.Format(emailEnvironmentHelper.NotificationsSettingsEmployerUrl, command.VacancyReview.HashedAccountId)
                && c.Tokens["VACcode"] == command.VacancyReview.VacancyReference.ToString()
                && c.Tokens["location"] == "Recruiting nationally"
            )
        ), Times.Once);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Exactly(2));
    }
    
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Command_Is_Referring_An_Employer_Created_Vacancy_Review_Then_Notifications_Sent_For_Employers_With_Immediate_Notification(
        UpsertVacancyReviewCommand command,
        RecruitUserApiResponse userApiResponse1,
        RecruitUserApiResponse userApiResponse2,
        RecruitUserApiResponse userApiResponse3,
        [Frozen] EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        UpsertVacancyReviewCommandHandler handler)
    {
        userApiResponse1.Name = "firstName lastName";
        userApiResponse1.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Immediately,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse2.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Weekly,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse3.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Daily,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        command.VacancyReview.ManualOutcome = "Referred";
        command.VacancyReview.OwnerType = "Employer";
        command.VacancyReview.EmployerLocationOption = AvailableWhere.AcrossEngland;
        var expectedPutRequest = new PutCreateVacancyReviewRequest(command.Id, command.VacancyReview);
        recruitApiClient.Setup(
                x => x.PutWithResponseCode<NullResponse>(
                    It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.Created, ""));
        var expectedGetUrl = new GetEmployerRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.AccountId);
        recruitApiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(
                It.Is<GetEmployerRecruitUserNotificationPreferencesApiRequest>(c =>
                    c.GetAllUrl.Equals(expectedGetUrl.GetAllUrl)))).ReturnsAsync([userApiResponse1, userApiResponse2, userApiResponse3]);

        await handler.Handle(command, CancellationToken.None);

        recruitApiClient.Verify(
            x => x.PutWithResponseCode<NullResponse>(
                It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)), Times.Once);
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == userApiResponse1.Email
                && c.TemplateId == emailEnvironmentHelper.VacancyReviewEmployerRejectedByDfeTemplateId
                && c.Tokens["advertTitle"] == command.VacancyReview.VacancyTitle
                && c.Tokens["firstName"] == userApiResponse1.FirstName
                && c.Tokens["employerName"] == command.VacancyReview.EmployerName
                && c.Tokens["rejectedAdvertURL"] == string.Format(emailEnvironmentHelper.ReviewVacancyReviewInRecruitEmployerUrl,command.VacancyReview.HashedAccountId, command.VacancyReview.VacancyId.ToString())
                && c.Tokens["notificationSettingsURL"] == string.Format(emailEnvironmentHelper.NotificationsSettingsEmployerUrl, command.VacancyReview.HashedAccountId)
                && c.Tokens["VACcode"] == command.VacancyReview.VacancyReference.ToString()
                && c.Tokens["location"] == "Recruiting nationally"
            )
        ), Times.Once);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }
    
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Command_Is_Referring_A_Provider_Created_Vacancy_Review_Then_Notifications_Sent_For_Employers_With_Immediate_Notification(
        UpsertVacancyReviewCommand command,
        RecruitUserApiResponse userApiResponse1,
        RecruitUserApiResponse userApiResponse2,
        RecruitUserApiResponse userApiResponse3,
        [Frozen] EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        UpsertVacancyReviewCommandHandler handler)
    {
        userApiResponse1.Name = "firstName lastName";
        userApiResponse1.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Immediately,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse2.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Weekly,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        userApiResponse3.NotificationPreferences.EventPreferences =
        [
            new EventPreference
            {
                Event = NotificationTypes.VacancyApprovedOrRejected,
                Frequency = NotificationFrequency.Daily,
                Method = "Email",
                Scope = NotificationScope.OrganisationVacancies
            }
        ];
        command.VacancyReview.ManualOutcome = "Referred";
        command.VacancyReview.OwnerType = "Provider";
        command.VacancyReview.EmployerLocationOption = AvailableWhere.AcrossEngland;
        var expectedPutRequest = new PutCreateVacancyReviewRequest(command.Id, command.VacancyReview);
        recruitApiClient.Setup(
                x => x.PutWithResponseCode<NullResponse>(
                    It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.Created, ""));
        var expectedGetUrl = new GetProviderRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.Ukprn);
        recruitApiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(
                It.Is<GetProviderRecruitUserNotificationPreferencesApiRequest>(c =>
                    c.GetAllUrl.Equals(expectedGetUrl.GetAllUrl)))).ReturnsAsync([userApiResponse1, userApiResponse2, userApiResponse3]);

        await handler.Handle(command, CancellationToken.None);

        recruitApiClient.Verify(
            x => x.PutWithResponseCode<NullResponse>(
                It.Is<PutCreateVacancyReviewRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)), Times.Once);
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == userApiResponse1.Email
                && c.TemplateId == emailEnvironmentHelper.VacancyReviewProviderRejectedByDfeTemplateId
                && c.Tokens["advertTitle"] == command.VacancyReview.VacancyTitle
                && c.Tokens["firstName"] == "firstName"
                && c.Tokens["employerName"] == command.VacancyReview.EmployerName
                && c.Tokens["rejectedAdvertURL"] == string.Format(emailEnvironmentHelper.ReviewVacancyReviewInRecruitProviderUrl,command.VacancyReview.Ukprn, command.VacancyReview.VacancyId.ToString())
                && c.Tokens["notificationSettingsURL"] == string.Format(emailEnvironmentHelper.NotificationsSettingsProviderUrl, command.VacancyReview.Ukprn)
                && c.Tokens["VACcode"] == command.VacancyReview.VacancyReference.ToString()
                && c.Tokens["location"] == "Recruiting nationally"
            )
        ), Times.Once);
        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }
}