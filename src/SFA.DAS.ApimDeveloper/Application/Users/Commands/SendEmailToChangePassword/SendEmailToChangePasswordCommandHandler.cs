using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.SendEmailToChangePassword
{
    public class SendEmailToChangePasswordCommandHandler : IRequestHandler<SendEmailToChangePasswordCommand, Unit>
    {
        private readonly ApimDeveloperMessagingConfiguration _config;
        private readonly INotificationService _notificationService;

        public SendEmailToChangePasswordCommandHandler(
            IOptions<ApimDeveloperMessagingConfiguration> options,
            INotificationService notificationService)
        {
            _config = options.Value;
            _notificationService = notificationService;
        }
        
        public async Task<Unit> Handle(SendEmailToChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var email = new ChangePasswordEmail(request, _config);
            var command = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);
            await _notificationService.Send(command);
            
            return Unit.Value;
        }
    }
}