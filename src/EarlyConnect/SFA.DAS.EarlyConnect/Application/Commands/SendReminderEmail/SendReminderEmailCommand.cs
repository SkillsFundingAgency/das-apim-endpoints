using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.SendReminderEmail
{
    public class SendReminderEmailCommand : IRequest<SendReminderEmailCommandResult>
    {
        public ReminderEmail EmailReminder { get; set; }
    }
}