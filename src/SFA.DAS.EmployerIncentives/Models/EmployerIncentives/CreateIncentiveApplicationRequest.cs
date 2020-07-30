using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerIncentives.Models.EmployerIncentives
{
    public class CreateIncentiveApplicationRequest
    {
        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public IEnumerable<IncentiveClaimApprenticeshipDto> Apprenticeships { get; set; }
    }
}
