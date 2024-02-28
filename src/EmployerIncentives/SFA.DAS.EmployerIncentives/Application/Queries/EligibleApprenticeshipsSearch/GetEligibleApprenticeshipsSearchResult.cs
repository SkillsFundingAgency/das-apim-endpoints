using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch
{
    public class GetEligibleApprenticeshipsSearchResult
    {
        public ApprenticeshipItem[] Apprentices { get; set; }
    }
}