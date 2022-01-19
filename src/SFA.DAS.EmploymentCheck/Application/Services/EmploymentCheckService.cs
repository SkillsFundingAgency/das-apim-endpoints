using SFA.DAS.EmploymentCheck.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmploymentCheck.Application.Services
{
    public class EmploymentCheckService : IEmploymentCheckService
    {
        private readonly IInternalApiClient<EmploymentCheckConfiguration> _client;

        public EmploymentCheckService(IInternalApiClient<EmploymentCheckConfiguration> client)
            => _client = client;
    }
}