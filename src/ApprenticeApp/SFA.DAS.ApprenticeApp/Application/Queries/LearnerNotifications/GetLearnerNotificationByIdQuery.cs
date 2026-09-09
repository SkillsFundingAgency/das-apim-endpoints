using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications
{
    public class GetLearnerNotificationByIdQuery : IRequest<GetLearnerNotificationByIdQueryResult>
    {
        public Guid AccountIdentifier { get; set; }
        public long NotificationIdentifier { get; set; }
    }
}