using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsRepeatReminderEmailsCommandHandler : IRequestHandler<SendBankDetailsRepeatReminderEmailsCommand>
    {
        private readonly IEmailService _emailService;

        public SendBankDetailsRepeatReminderEmailsCommandHandler(IEmailService employerIncentivesService)
        {
            _emailService = employerIncentivesService;
        }
    
        public async Task<Unit> Handle(SendBankDetailsRepeatReminderEmailsCommand command, CancellationToken cancellationToken)
        {
            var request = new SendBankDetailsRepeatReminderEmailsRequest(command.ApplicationCutOffDate);
            await _emailService.TriggerBankRepeatReminderEmails(request);

            return Unit.Value;
        }
    }
}
