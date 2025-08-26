using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.ApplicationReview.Events.ApplicationReviewShared;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Command;

[TestFixture]
internal class WhenHandlingApplicationReviewSharedCommand
{
    [Test, MoqAutoData]
    public async Task Then_If_The_Command_Is_Handled_Then_Notifications_Sent_For_Employers_With_Immediate_Notification(
        ApplicationReviewSharedEvent command,
        RecruitUserApiResponse userApiResponse1,
        RecruitUserApiResponse userApiResponse2,
        RecruitUserApiResponse userApiResponse3,
        [Frozen] EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ApplicationReviewSharedEventHandler handler)
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
        
        recruitApiClient
            .Setup(x => x.GetAll<RecruitUserApiResponse>(
                It.Is<GetEmployerRecruitUserNotificationPreferencesApiRequest>(c =>
                    c.GetAllUrl.Contains(command.AccountId.ToString())))).ReturnsAsync([userApiResponse1, userApiResponse2, userApiResponse3]);

        await handler.Handle(command, CancellationToken.None);

        var employerReviewUrl = $"{emailEnvironmentHelper.ApplicationReviewSharedEmployerUrl}"
            .Replace("{0}", command.HashAccountId)
            .Replace("{1}", command.VacancyId.ToString())
            .Replace("{2}", command.ApplicationId.ToString());

        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.TemplateId == emailEnvironmentHelper.ApplicationReviewSharedEmailTemplatedId
                && c.RecipientsAddress == userApiResponse1.Email
                && c.Tokens["firstName"] == userApiResponse1.Name
                && c.Tokens["trainingProvider"] == command.TrainingProvider
                && c.Tokens["advertTitle"] == command.AdvertTitle
                && c.Tokens["vacancyReference"] == command.VacancyReference.ToString()
                && c.Tokens["applicationUrl"] == employerReviewUrl)
        ), Times.Once);

        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
    }
}