using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsReminderEmailCommandHandler : IRequestHandler<SendBankDetailsReminderEmailCommand>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public SendBankDetailsReminderEmailCommandHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(SendBankDetailsReminderEmailCommand command, CancellationToken cancellationToken)
        {
            var request = new SendBankDetailsEmailRequest(command.AccountId, command.AccountLegalEntityId, command.EmailAddress, command.AddBankDetailsUrl);

            await _employerIncentivesService.SendBankDetailReminderEmail(command.AccountId, request);

            return Unit.Value;
        }
    }
}
