using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.AuthenticateUser
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserCommandResult>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public AuthenticateUserCommandHandler (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        public async Task<AuthenticateUserCommandResult> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _apimDeveloperApiClient.PostWithResponseCode<PostAuthenticateUserResult>(
                    new PostAuthenticateUserRequest(request.Email, request.Password));

            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)apiResponse.StatusCode} ({apiResponse.StatusCode})", apiResponse.StatusCode, apiResponse.ErrorContent);
            }
            
            return new AuthenticateUserCommandResult
            {
                User = apiResponse.Body
            };
        }
    }
}