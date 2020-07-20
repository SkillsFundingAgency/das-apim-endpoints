using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Models.Commitments
{
    public class ApprenticeshipSearchResponse
    {
        public IEnumerable<ApprenticeshipItem> Apprenticeships { get; set; }
    }
}