using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class GetApprenticeshipListResponse
    {
        public IEnumerable<ApprenticeshipItem> Apprenticeships { get; set; }
    }
}