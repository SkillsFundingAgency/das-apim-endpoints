using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PutIncentiveApplicationRequest : IPutApiRequest
    {
        public string BaseUrl { get; set; }
        public string PutUrl => $"{BaseUrl}applications";
        public object Data { get; set; }
    }

    public class UpdateIncentiveApplicationRequest
    {
        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public IncentiveClaimApprenticeshipDto[] Apprenticeships { get; set; }
    }
}
