using System.Net;
using System.Threading;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.VacancyReview.Commands;

public class WhenHandlingUpsertVacancyReviewCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Called_For_Providers(
        UpsertVacancyReviewCommand command,
        RecruitUserApiResponse userApiResponse1,
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
        var expectedGetUrl = new GetProviderRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.Ukprn,
            NotificationTypes.VacancyApprovedOrRejected);
        recruitApiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(
                It.Is<GetProviderRecruitUserNotificationPreferencesApiRequest>(c =>
                    c.GetAllUrl.Equals(expectedGetUrl.GetAllUrl)))).ReturnsAsync([userApiResponse1, userApiResponse3]);

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
        var expectedGetUrl = new GetEmployerRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.AccountId,
            NotificationTypes.VacancyApprovedOrRejected);
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
        recruitApiClient
            .Verify(x => x.GetAll<RecruitUserApiResponse>(
                It.IsAny<GetProviderRecruitUserNotificationPreferencesApiRequest>()), Times.Never);
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
        var expectedGetUrl = new GetEmployerRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.AccountId,
                NotificationTypes.VacancyApprovedOrRejected);
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
        var expectedGetUrl = new GetProviderRecruitUserNotificationPreferencesApiRequest(command.VacancyReview.Ukprn,
                NotificationTypes.VacancyApprovedOrRejected);
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