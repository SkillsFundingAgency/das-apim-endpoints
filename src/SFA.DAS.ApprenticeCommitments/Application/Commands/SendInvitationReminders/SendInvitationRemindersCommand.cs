using System;
using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.SendInvitationReminders
{
    public class SendInvitationRemindersCommand : IRequest
    {
        public DateTime InvitationCutOffTime { set; get; }
    }
}