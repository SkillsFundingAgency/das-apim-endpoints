using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsRequiredEmailCommandHandler : IRequestHandler<SendBankDetailsRequiredEmailCommand>
    {
        private readonly IEmailService _emailService;

        public SendBankDetailsRequiredEmailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendBankDetailsRequiredEmailCommand command, CancellationToken cancellationToken)
        {
            var request = new PostBankDetailsRequiredEmailRequest
            {
                Data = new SendBankDetailsEmailRequest(command.AccountId, command.AccountLegalEntityId, command.EmailAddress, command.AddBankDetailsUrl)
            };

            await _emailService.SendEmail(request);

            return Unit.Value;
        }
    }
}
