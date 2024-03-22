using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsRepeatReminderEmailsCommand : IRequest<Unit>
    {
        public DateTime ApplicationCutOffDate { get; }

        public SendBankDetailsRepeatReminderEmailsCommand(DateTime applicatioCutOffDate)
        {
            ApplicationCutOffDate = applicatioCutOffDate;
        }
    }
}
