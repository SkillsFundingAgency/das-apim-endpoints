using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PostEmployerDemandNotificationAuditRequest : IPostApiRequest
    {
        private readonly Guid _id;
        private readonly Guid _courseDemandId;
        private readonly NotificationType _notificationType;

        public PostEmployerDemandNotificationAuditRequest( Guid id, Guid courseDemandId, NotificationType notificationType)
        {
            _id = id;
            _courseDemandId = courseDemandId;
            _notificationType = notificationType;
        }

        public string PostUrl => $"api/Demand/{_courseDemandId}/notification-audit/{_id}?notificationType={(short)_notificationType}";
        public object Data { get; set; }
    }

    public enum NotificationType
    {
        Reminder = 0,
        StoppedByUser = 1,
        StoppedAutomaticCutOff = 2,
        StoppedCourseClosed = 3
    }
}