using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

public class ApplicationCreatedEmailCommandHandler(
    ILevyTransferMatchingService levyTransferMatchingService,
    ILogger<ApplicationCreatedEmailCommandHandler> logger,
     INotificationService notificationService,
    IAccountsService accountsService
    ) : IRequestHandler<ApplicationCreatedEmailCommand, Unit>
{    
    public async Task<Unit> Handle(ApplicationCreatedEmailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending email for application {applicationId} created to receiver {receiverId} for Pledge {pledgeId}",
                   request.ApplicationId, request.ReceiverId, request.PledgeId);

        var getApplicationTask = levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));
        var getAccountUsersTask = accountsService.GetAccountUsers(request.ReceiverId);

        await Task.WhenAll(getApplicationTask, getAccountUsersTask);

        var application = getApplicationTask.Result;
        var users = getAccountUsersTask.Result.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();

        foreach (var user in users)
        {
            var templateID = "";
            if (application.MatchPercentage < 100)
            {
                templateID = "PartialMatchApplicationCreated";
            }
            else if (application.MatchPercentage == 100 && application.AutomaticApprovalOption == AutomaticApprovalOption.DelayedAutoApproval)
            {
                templateID = "ApplicationCreatedForDelayedPledge";
            }
            if (!string.IsNullOrEmpty(templateID))
            {
                var email = new ApplicationCreatedEmail(user.Email, application.EmployerAccountName, request.EncodedApplicationId, request.UnsubscribeUrl, templateID);
                var command = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);

                logger.LogInformation("Sending {templateID} email for application {applicationId}", templateID, request.ApplicationId);
                await notificationService.Send(command);
            }               
        }

        return Unit.Value;
    }
}