using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PostEmployerDemandNotificationAuditRequest : IPostApiRequest<object>
    {
        private readonly Guid _id;
        private readonly Guid _courseDemandId;

        public PostEmployerDemandNotificationAuditRequest(Guid id, Guid courseDemandId)
        {
            _id = id;
            _courseDemandId = courseDemandId;
        }

        public string PostUrl => $"api/Demand/{_courseDemandId}/notification-audit/{_id}";
        public object Data { get; set; }
    }
}