using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class CreateIncentiveApplicationRequestData
    {
        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public IncentiveClaimApprenticeshipDto[] Apprenticeships { get; set; }
    }
}
