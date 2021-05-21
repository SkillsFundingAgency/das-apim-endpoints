using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class CollectionCalendarService : ICollectionCalendarService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public CollectionCalendarService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task UpdateCollectionCalendarPeriod(UpdateCollectionCalendarPeriodRequestData requestData)
        {
            await _client.Patch<UpdateCollectionCalendarPeriodRequestData>(new UpdateCollectionCalendarPeriodRequest { Data = requestData });
        }
    }
}
