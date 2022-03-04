using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;

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

        public async Task<ApprenticeshipIncentiveDto[]> GetApprenticeshipIncentives(long accountId, long accountLegalEntityId)
        {
            var response = await _client.GetAll<ApprenticeshipIncentiveDto>(new GetApprenticeshipIncentivesRequest(accountId, accountLegalEntityId));

            return response.ToArray();
        }

        public async Task RecalculateEarnings(RecalculateEarningsRequest recalculateEarningsRequest)
        {
            var postRequest = new PostRecalculateEarningsRequest(recalculateEarningsRequest);

            await _client.PostWithResponseCode<PostRecalculateEarningsRequest>(postRequest);
        }
    }
}