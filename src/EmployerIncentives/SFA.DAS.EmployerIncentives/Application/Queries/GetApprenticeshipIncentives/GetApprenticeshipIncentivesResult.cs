using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApprenticeshipIncentives
{
    public class GetApprenticeshipIncentivesResult
    {
        public IEnumerable<ApprenticeshipIncentiveDto> ApprenticeshipIncentives { get ; set ; }
    }
}