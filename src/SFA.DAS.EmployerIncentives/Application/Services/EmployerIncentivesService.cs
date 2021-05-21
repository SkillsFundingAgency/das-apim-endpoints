using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EarningsResilienceCheck;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class EmployerIncentivesService : IEmployerIncentivesService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public EmployerIncentivesService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _client.GetResponseCode(new GetHealthRequest());
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        public async Task<GetIncentiveDetailsResponse> GetIncentiveDetails()
        {
            return await _client.Get<GetIncentiveDetailsResponse>(new GetIncentiveDetailsRequest());
        }

        public async Task UpdateCollectionCalendarPeriod(UpdateCollectionCalendarPeriodRequestData requestData)
        {
            await _client.Patch<UpdateCollectionCalendarPeriodRequestData>(new UpdateCollectionCalendarPeriodRequest { Data = requestData });
        }

    }
}