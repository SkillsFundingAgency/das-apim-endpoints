using MediatR;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.EmailTemplates;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.ApplicationReviewShared;
// <summary>
// Handles the command to share an application review via email.
// </summary>
public class ApplicationReviewSharedCommandHandler(
    INotificationService notificationService,
    EmailEnvironmentHelper helper) : IRequestHandler<ApplicationReviewSharedCommand>
{
    public async Task Handle(ApplicationReviewSharedCommand request, CancellationToken cancellationToken)
    {
        var employerReviewUrl = $"{helper.ApplicationReviewSharedEmployerUrl}"
            .Replace("{0}", request.HashAccountId)
            .Replace("{1}", request.VacancyId.ToString())
            .Replace("{2}", request.ApplicationId.ToString());

        var applicationReviewSharedEmail = new ApplicationReviewSharedEmailTemplate(helper.ApplicationReviewSharedEmailTemplatedId,
            request.RecipientEmail,
            request.FirstName,
            request.TrainingProvider,
            request.AdvertTitle,
            request.VacancyReference.ToString(),
            employerReviewUrl);

        var sendEmailCommand = new SendEmailCommand(applicationReviewSharedEmail.TemplateId,
            applicationReviewSharedEmail.RecipientAddress,
            applicationReviewSharedEmail.Tokens);

        // Send the email using the notification service
        await notificationService.Send(sendEmailCommand).ConfigureAwait(false);
    }
}