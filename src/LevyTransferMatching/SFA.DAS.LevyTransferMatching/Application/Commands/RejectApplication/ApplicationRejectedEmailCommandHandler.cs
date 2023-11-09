using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class ApplicationRejectedEmailCommandHandler : IRequestHandler<ApplicationRejectedEmailCommand>
    {
        private readonly IAccountsService _accountsService;
        private readonly ILogger<ApplicationRejectedEmailCommandHandler> _logger;
        private readonly INotificationService _notificationService;

        public ApplicationRejectedEmailCommandHandler(ILogger<ApplicationRejectedEmailCommandHandler> logger, 
            INotificationService notificationService, IAccountsService accountsService)
        {
            _accountsService = accountsService;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(ApplicationRejectedEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sending email for application {request.ApplicationId} Rejected to receiver {request.ReceiverId} for Pledge {request.PledgeId}");

            var getAccountUsersTask =  await _accountsService.GetAccountUsers(request.ReceiverId);

            var users = getAccountUsersTask.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();

            foreach (var user in users)
            {             
                var email = new ApplicationRejectedEmail(user.Email, user.Name, request.EncodedApplicationId, request.BaseUrl);
                var command = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);
                await _notificationService.Send(command);
            }

            return Unit.Value;
        }
    }
}