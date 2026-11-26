using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications
{
    public class AddApprenticeshipProgressCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
    }
}
