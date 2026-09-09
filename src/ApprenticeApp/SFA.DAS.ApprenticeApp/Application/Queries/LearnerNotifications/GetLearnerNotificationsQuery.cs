using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications
{
    public class GetLearnerNotificationsQuery : IRequest<GetLearnerNotificationsQueryResult>
    {
        public Guid AccountIdentifier { get; set; }
        public string Order { get; set; }
        public DateTime? DateFrom { get; set; }
        public List<int> Statuses { get; set; }
    }
}