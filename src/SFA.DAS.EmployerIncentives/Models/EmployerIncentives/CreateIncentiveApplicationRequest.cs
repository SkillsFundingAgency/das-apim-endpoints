using System;

namespace SFA.DAS.EmployerIncentives.Models.EmployerIncentives
{
    public class CreateIncentiveApplicationRequest
    {
        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public IncentiveClaimApprenticeshipDto[] Apprenticeships { get; set; }
    }
}
