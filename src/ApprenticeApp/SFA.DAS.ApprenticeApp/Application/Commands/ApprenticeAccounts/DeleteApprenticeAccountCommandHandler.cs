using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class DeleteApprenticeAccountCommandHandler : IRequestHandler<DeleteApprenticeAccountCommand, DeleteApprenticeAccountResponse>
    {        
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;        

        public DeleteApprenticeAccountCommandHandler(            
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient
            )
        {
            _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
        }

        public async Task<DeleteApprenticeAccountResponse> Handle(DeleteApprenticeAccountCommand request, CancellationToken cancellationToken)
        {
            var result = await _apprenticeAccountsApiClient
                .DeleteWithResponseCode<DeleteApprenticeAccountResponse>(
                new DeleteApprenticeAccountRequest(request.ApprenticeId),
                includeResponse: true);

            return result.Body;
        }
    }
}
