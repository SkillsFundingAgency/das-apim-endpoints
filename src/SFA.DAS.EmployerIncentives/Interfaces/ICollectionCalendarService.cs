using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ICollectionCalendarService
    {
        Task UpdateCollectionCalendarPeriod(UpdateCollectionCalendarPeriodRequestData requestData);
    }
}
