using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments
{
    public class ApprenticeshipSearchResponse
    {
        public IEnumerable<ApprenticeshipItem> Apprenticeships { get; set; }
    }
}