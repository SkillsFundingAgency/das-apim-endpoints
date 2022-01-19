using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmploymentCheck.Application.Services
{
    public class EmploymentCheckService : IEmploymentCheckService
    {
        private readonly IInternalApiClient<EmploymentCheckApiConfiguration> _client;

        public EmploymentCheckService(IInternalApiClient<EmploymentCheckApiConfiguration> client)
            => _client = client;

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client);

    }
}