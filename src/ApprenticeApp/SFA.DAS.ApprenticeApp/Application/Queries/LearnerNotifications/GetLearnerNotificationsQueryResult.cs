using SFA.DAS.ApprenticeApp.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications
{
    public class GetLearnerNotificationsQueryResult
    {
        public List<LearnerNotification> Notifications { get; set; }
    }
}