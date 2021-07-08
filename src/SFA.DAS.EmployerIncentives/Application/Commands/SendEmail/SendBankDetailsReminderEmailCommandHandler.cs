using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsReminderEmailCommandHandler : IRequestHandler<SendBankDetailsReminderEmailCommand>
    {
        private readonly IEmailService _emailService;

        public SendBankDetailsReminderEmailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendBankDetailsReminderEmailCommand command, CancellationToken cancellationToken)
        {
            var request = new PostBankDetailsReminderEmailRequest 
            {
                Data = new SendBankDetailsEmailRequest(command.AccountId, command.AccountLegalEntityId, command.EmailAddress, command.AddBankDetailsUrl)
            };

            await _emailService.SendEmail(request);

            return Unit.Value;
        }
    }
}
