using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome;

public class ReceiverApplicationApprovedEmailCommandHandler(
    IAccountsService accountsService,
    ILogger<ReceiverApplicationApprovedEmailCommandHandler> logger,
    INotificationService notificationService)
    : IRequestHandler<ReceiverApplicationApprovedEmailCommand, Unit>
{
    public async Task<Unit> Handle(ReceiverApplicationApprovedEmailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending email for application {applicationId} approved to receiver {receiverId} for Pledge {pledgeId}",
            request.ApplicationId, request.ReceiverId, request.PledgeId);

        var getAccountUsersTask = accountsService.GetAccountUsers(request.ReceiverId);
        var getAccountTask = accountsService.GetAccount(request.EncodedAccountId);

        await Task.WhenAll(getAccountUsersTask, getAccountTask);

        var accountUsers = await getAccountUsersTask;
        var users = accountUsers.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();

        var account = await getAccountTask;

        foreach (var user in users)
        {
            var email = new ReceiverApplicationApprovedEmail(
                user.Email,
                account.DasAccountName,
                request.EncodedApplicationId,
                request.BaseUrl,
                account.EncodedAccountId,
                request.UnsubscribeNotificationsUrl);

            var command = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);

            await notificationService.Send(command);
        }

        return Unit.Value;
    }
}