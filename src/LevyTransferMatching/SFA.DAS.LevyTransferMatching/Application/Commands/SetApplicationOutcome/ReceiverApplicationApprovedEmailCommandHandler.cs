using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome
{
    public class ReceiverApplicationApprovedEmailCommandHandler : IRequestHandler<ReceiverApplicationApprovedEmailCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IAccountsService _accountsService;
        private readonly ILogger<SetApplicationOutcomeCommandHandler> _logger;
        private readonly INotificationService _notificationService;

        public ReceiverApplicationApprovedEmailCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<SetApplicationOutcomeCommandHandler> logger, 
            INotificationService notificationService, IAccountsService accountsService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _accountsService = accountsService;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(ReceiverApplicationApprovedEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sending email for application {request.ApplicationId} approved to receiver {request.ReceiverId} for Pledge {request.PledgeId}");

            var getApplicationTask = _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));
            var getAccountUsersTask = _accountsService.GetAccountUsers(request.ReceiverId);

            await Task.WhenAll(getApplicationTask, getAccountUsersTask);

            var application = getApplicationTask.Result;
            var users = getAccountUsersTask.Result.Where(x => (x.Role == "Owner" || x.Role == "Transactor") && x.CanReceiveNotifications).ToList();

            foreach (var user in users)
            {
                var email = new ReceiverApplicationApprovedEmail(user.Email, application.EmployerAccountName, application.SenderEmployerAccountName, request.BaseUrl, request.ReceiverEncodedAccountId);

                var command = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);

                await _notificationService.Send(command);
            }

            return Unit.Value;
        }
    }
}