using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesService
    {
        Task<bool> IsHealthy();
        Task<GetIncentiveDetailsResponse> GetIncentiveDetails();
        Task UpdateCollectionCalendarPeriod(UpdateCollectionCalendarPeriodRequestData requestData);
    }
}