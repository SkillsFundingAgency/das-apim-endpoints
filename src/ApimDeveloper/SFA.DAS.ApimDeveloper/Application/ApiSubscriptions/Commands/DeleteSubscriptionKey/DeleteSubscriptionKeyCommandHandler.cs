using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.DeleteSubscriptionKey
{
    public class DeleteSubscriptionKeyCommandHandler : IRequestHandler<DeleteSubscriptionKeyCommand, Unit>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public DeleteSubscriptionKeyCommandHandler(IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }

        public async Task<Unit> Handle(DeleteSubscriptionKeyCommand command, CancellationToken cancellationToken)
        {
            var request = new DeleteSubscriptionKeyRequest(command.AccountIdentifier, command.ProductId);
            await _apimDeveloperApiClient.Delete(request);
            return Unit.Value;
        }
    }
}
