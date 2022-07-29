using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EarningsResilienceCheck;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class EarningsResilienceCheckService : IEarningsResilienceCheckService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public EarningsResilienceCheckService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task RunCheck()
        {
            await _client.PostWithResponseCode<string>(new EarningsResilenceCheckRequest(), false);
        }
    }
}
