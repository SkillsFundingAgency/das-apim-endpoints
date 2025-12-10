using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class DeleteApprenticeAccountCommandHandler : IRequestHandler<DeleteApprenticeAccountCommand, Unit>
    {        
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;        

        public DeleteApprenticeAccountCommandHandler(            
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient
            )
        {
            _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
        }

        public async Task<Unit> Handle(DeleteApprenticeAccountCommand request, CancellationToken cancellationToken)
        {
            await _apprenticeAccountsApiClient.Delete(new DeleteApprenticeAccountRequest(request.ApprenticeId));
            return Unit.Value;

        }
    }
}
