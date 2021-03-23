using System;
using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.SendInvitationReminders
{
    public class SendInvitationRemindersCommand : IRequest
    {
        public DateTime SendNow { set; get; }
        public int RemindAfterDays { set; get; }
    }
}