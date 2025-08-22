using MediatR;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.EmailTemplates;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Messages;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.ApplicationReviewShared;
// <summary>
// Handles the command to share an application review via email.
// </summary>
public class ApplicationReviewSharedCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    INotificationService notificationService,
    EmailEnvironmentHelper helper) : IRequestHandler<ApplicationReviewSharedCommand>
{
    public async Task Handle(ApplicationReviewSharedCommand request, CancellationToken cancellationToken)
    {
        var employerReviewUrl = $"{helper.ApplicationReviewSharedEmployerUrl}"
            .Replace("{0}", request.HashAccountId)
            .Replace("{1}", request.VacancyId.ToString())
            .Replace("{2}", request.ApplicationId.ToString());

        var users = await apiClient.GetAll<RecruitUserApiResponse>(
            new GetEmployerRecruitUserNotificationPreferencesApiRequest(request.AccountId, NotificationTypes.ApplicationSharedWithEmployer));

        var usersToNotify = users.Where(user => user.NotificationPreferences.EventPreferences.Any(c =>
            c.Frequency.Equals(NotificationFrequency.Immediately))).ToList();

        var emailTasks = usersToNotify
            .Select(apiResponse => ApplicationReviewSharedEmailTemplate(request, apiResponse, employerReviewUrl))
            .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
            .Select(notificationService.Send).ToList();

        await Task.WhenAll(emailTasks);
    }

    private EmailTemplateArguments ApplicationReviewSharedEmailTemplate(ApplicationReviewSharedCommand request, RecruitUserApiResponse apiResponse, string employerReviewUrl)
    {
        return new ApplicationReviewSharedEmailTemplate(helper.ApplicationReviewSharedEmailTemplatedId,
            apiResponse.Email,
            apiResponse.Name,
            request.TrainingProvider,
            request.AdvertTitle,
            request.VacancyReference.ToString(),
            employerReviewUrl);
    }
}