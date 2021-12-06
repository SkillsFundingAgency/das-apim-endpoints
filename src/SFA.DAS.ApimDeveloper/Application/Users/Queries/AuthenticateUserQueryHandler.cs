using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.Users.Queries
{
    public class AuthenticateUserQueryHandler : IRequestHandler<AuthenticateUserQuery, AuthenticateUserQueryResult>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public AuthenticateUserQueryHandler (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        public async Task<AuthenticateUserQueryResult> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var actual =
                await _apimDeveloperApiClient.Get<GetAuthenticateUserResult>(
                    new GetAuthenticateUserRequest(request.Email, request.Password));

            return new AuthenticateUserQueryResult
            {
                User = actual
            };
        }
    }
}