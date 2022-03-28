using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class EmploymentCheckService : IEmploymentCheckService
    {
        private readonly IInternalApiClient<EmploymentCheckConfiguration> _client;

        public EmploymentCheckService(IInternalApiClient<EmploymentCheckConfiguration> client)
        {
            _client = client;
        }

        public async Task<RegisterEmploymentCheckResponse> Register(RegisterEmploymentCheckRequest request)
        {
            var response = await _client.PostWithResponseCode<RegisterEmploymentCheckResponse>(request);

            if (ApiResponseErrorChecking.IsSuccessStatusCode(response.StatusCode)) return response.Body;

            throw new HttpRequestContentException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})", response.StatusCode, response.ErrorContent);
        }
    }
}
