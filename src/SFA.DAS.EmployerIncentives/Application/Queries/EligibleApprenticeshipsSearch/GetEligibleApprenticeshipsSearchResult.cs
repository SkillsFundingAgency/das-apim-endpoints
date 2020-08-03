using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.EmployerIncentives.Models.Commitments;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch
{
    public class GetEligibleApprenticeshipsSearchResult
    {
        public ApprenticeshipItem[] Apprentices { get; set; }
    }
}