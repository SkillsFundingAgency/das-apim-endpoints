using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck
{
    public class RegisterEmploymentCheckCommandHandler : IRequestHandler<RegisterEmploymentCheckCommand, RegisterEmploymentCheckResponse>
    {
        private readonly IInternalApiClient<EmploymentCheckConfiguration> _client;

        public RegisterEmploymentCheckCommandHandler(IInternalApiClient<EmploymentCheckConfiguration> client)
        {
            _client = client;
        }

        public async Task<RegisterEmploymentCheckResponse> Handle(RegisterEmploymentCheckCommand command, CancellationToken cancellationToken)
        {
            var response = await _client.PostWithResponseCode<RegisterEmploymentCheckResponse>(new RegisterEmploymentCheckRequest(command));

            if (ApiResponseErrorChecking.IsSuccessStatusCode(response.StatusCode)) return response.Body;

            throw new HttpRequestContentException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})", response.StatusCode, response.ErrorContent);
        }
    }
}
