using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class ConfirmIncentiveApplicationRequest : IPatchApiRequest
    {
        public ConfirmIncentiveApplicationRequest(Guid applicationId, long accountId, DateTime dateSubmitted)
        {
            IncentiveApplicationId = applicationId;
            AccountId = accountId;
            DateSubmitted = dateSubmitted;
        }
        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string BaseUrl { get; set; }
        public string PatchUrl => $"{BaseUrl}applications/{IncentiveApplicationId}";
        public object Data { get; set; }
    }
}
