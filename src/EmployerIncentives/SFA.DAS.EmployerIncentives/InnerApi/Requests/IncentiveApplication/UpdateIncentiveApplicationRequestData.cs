using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class UpdateIncentiveApplicationRequestData
    {
        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public IncentiveClaimApprenticeshipDto[] Apprenticeships { get; set; }
    }
}