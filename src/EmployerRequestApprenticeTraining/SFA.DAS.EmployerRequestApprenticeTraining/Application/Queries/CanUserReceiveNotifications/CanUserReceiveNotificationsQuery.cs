using MediatR;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.CanUserReceiveNotifications
{
    public class CanUserReceiveNotificationsQuery : IRequest<bool>
    {
        public long AccountId { get; set; }
        public Guid UserId { get; set; }
    }
}