using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications
{
    public class GetLearnerNotificationsQuery : IRequest<GetLearnerNotificationsQueryResult>
    {
        public Guid AccountIdentifier { get; set; }
    }
}