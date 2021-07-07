using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetEmployerDemandByExpiredDemandRequest : IGetApiRequest
    {
        private readonly Guid _expiredCourseDemandId;

        public GetEmployerDemandByExpiredDemandRequest (Guid expiredCourseDemandId)
        {
            _expiredCourseDemandId = expiredCourseDemandId;
        }
        public string GetUrl => $"api/demand?expiredCourseDemandId={_expiredCourseDemandId}";
    }
}