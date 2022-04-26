using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class IncentivesEmploymentCheckService : IIncentivesEmploymentCheckService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public IncentivesEmploymentCheckService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task Update(UpdateRequest updateRequest)
        {
            var request = new UpdateEmploymentCheckRequest { Data = updateRequest };
            await _client.Put(request);
        }
    }
}
