using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class PatchMyApprenticeshipCommandHandler : IRequestHandler<PatchMyApprenticeshipCommand, bool>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _client;

        public PatchMyApprenticeshipCommandHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<bool> Handle(PatchMyApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            var patchRequest = new PatchMyApprenticeshipRequest(request.ApprenticeId, request.PatchData);

            await _client.Patch(patchRequest);

            return true;
        }
    }
}
