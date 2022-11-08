using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesService
    {
        Task<bool> IsHealthy();
        Task<GetIncentiveDetailsResponse> GetIncentiveDetails();
        Task<ApprenticeshipIncentiveDto[]> GetApprenticeshipIncentives(long accountId, long accountLegalEntityId);
        Task RecalculateEarnings(RecalculateEarningsRequest recalculateEarningsRequest);
        Task RevertPayments(RevertPaymentsRequest revertPaymentsRequest);
        Task ReinstatePayments(ReinstatePaymentsRequest reinstatePaymentsRequest);
    }
}