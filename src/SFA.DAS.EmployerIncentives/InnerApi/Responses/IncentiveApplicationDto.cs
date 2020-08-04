using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class IncentiveApplicationDto
    {
        public long AccountLegalEntityId { get; set; }
        public IEnumerable<IncentiveApplicationApprenticeshipDto> Apprenticeships { get; set; }
    }
}
