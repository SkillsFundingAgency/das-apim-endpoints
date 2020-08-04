using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class IncentiveApplicationDto
    {
        public IEnumerable<IncentiveApplicationApprenticeshipDto> Apprenticeships { get; set; }
    }
}
