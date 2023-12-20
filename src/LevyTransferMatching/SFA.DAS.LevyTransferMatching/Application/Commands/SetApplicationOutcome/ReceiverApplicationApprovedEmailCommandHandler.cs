using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome
{
    public class ReceiverApplicationApprovedEmailCommandHandler : IRequestHandler<ReceiverApplicationApprovedEmailCommand>
    {
        private readonly IAccountsService _accountsService;
        private readonly ILogger<ReceiverApplicationApprovedEmailCommandHandler> _logger;
        private readonly INotificationService _notificationService;

        public ReceiverApplicationApprovedEmailCommandHandler(ILogger<ReceiverApplicationApprovedEmailCommandHandler> logger, 
            INotificationService notificationService, IAccountsService accountsService)
        {
            _accountsService = accountsService;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(ReceiverApplicationApprovedEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sending email for application {request.ApplicationId} approved to receiver {request.ReceiverId} for Pledge {request.PledgeId}");

            var getAccountUsersTask = await _accountsService.GetAccountUsers(request.ReceiverId);

            var users = getAccountUsersTask.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();

            foreach (var user in users)
            {
                var email = new ReceiverApplicationApprovedEmail(user.Email, user.Name, request.EncodedApplicationId);

                var command = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);

                await _notificationService.Send(command);
            }

            return Unit.Value;
        }
    }
}