using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications
{
    public class DeleteLearnerNotificationCommand : IRequest<Unit>
    {
        public Guid AccountIdentifier { get; set; }
        public long NotificationIdentifier { get; set; }
    }
}