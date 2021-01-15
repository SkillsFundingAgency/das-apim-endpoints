using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsRepeatReminderEmailsCommandHandler : IRequestHandler<SendBankDetailsRepeatReminderEmailsCommand>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public SendBankDetailsRepeatReminderEmailsCommandHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
    
        public async Task<Unit> Handle(SendBankDetailsRepeatReminderEmailsCommand command, CancellationToken cancellationToken)
        {
            var request = new SendBankDetailsRepeatReminderEmailsRequest(command.ApplicationCutOffDate);
            await _employerIncentivesService.SendBankDetailsRepeatReminderEmails(request);

            return Unit.Value;
        }
    }
}
