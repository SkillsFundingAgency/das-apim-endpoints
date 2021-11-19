using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.RenewSubscriptionKey
{
    public class RenewSubscriptionKeyCommandHandler : IRequestHandler<RenewSubscriptionKeyCommand, Unit>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public RenewSubscriptionKeyCommandHandler(IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        
        public async Task<Unit> Handle(RenewSubscriptionKeyCommand command, CancellationToken cancellationToken)
        {
            var request = new PostRenewSubscriptionKeyRequest(command.AccountIdentifier, command.ProductId);
            await _apimDeveloperApiClient.PostWithResponseCode<string>(request);
            return Unit.Value;
        }
    }
}