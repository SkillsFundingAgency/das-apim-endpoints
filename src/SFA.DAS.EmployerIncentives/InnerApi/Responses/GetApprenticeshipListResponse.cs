using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class GetApprenticeshipListResponse
    {
        public IEnumerable<ApprenticeshipItem> Apprenticeships { get; set; }
    }
}