using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsRequiredEmailCommandHandler : IRequestHandler<SendBankDetailsRequiredEmailCommand>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public SendBankDetailsRequiredEmailCommandHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(SendBankDetailsRequiredEmailCommand command, CancellationToken cancellationToken)
        {
            var request = new SendBankDetailsEmailRequest(command.AccountId, command.AccountLegalEntityId, command.EmailAddress, command.AddBankDetailsUrl);

            await _employerIncentivesService.SendBankDetailRequiredEmail(command.AccountId, request);

            return Unit.Value;
        }
    }
}
