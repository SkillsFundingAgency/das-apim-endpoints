using MediatR;
using SFA.DAS.EmploymentCheck.Configuration;
using SFA.DAS.EmploymentCheck.Requests;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Application.Commands.RegisterCheck
{
    public class RegisterCheckCommandHandler : IRequestHandler<RegisterCheckCommand, RegisterCheckResponse>
    {
        private readonly IInternalApiClient<EmploymentCheckConfiguration> _client;

        public RegisterCheckCommandHandler(IInternalApiClient<EmploymentCheckConfiguration> client)
        {
            _client = client;
        }

        public async Task<RegisterCheckResponse> Handle(RegisterCheckCommand command,
            CancellationToken cancellationToken)
        {
            var response = await _client.PostWithResponseCode<RegisterCheckResponse>(new RegisterCheckRequest(command));
            
            if (ApiResponseErrorChecking.IsSuccessStatusCode(response.StatusCode)) return response.Body;

            throw new HttpRequestContentException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})", response.StatusCode, response.ErrorContent);
        }
    }
}