using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class IncentiveApplicationDto
    {
        public long AccountLegalEntityId { get; set; }
        public IEnumerable<IncentiveApplicationApprenticeshipDto> Apprenticeships { get; set; }

        public static implicit operator IncentiveApplicationDto(IncentiveApplication source)
        {
            return new IncentiveApplicationDto
            {
                AccountLegalEntityId = source.AccountLegalEntityId,
                Apprenticeships = source.Apprenticeships.Select(x => (IncentiveApplicationApprenticeshipDto)x)
            };
        }
    }
}
