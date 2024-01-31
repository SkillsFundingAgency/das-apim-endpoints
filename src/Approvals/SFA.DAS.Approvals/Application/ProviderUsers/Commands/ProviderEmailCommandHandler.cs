using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.ProviderUsers.Commands
{
    public class ProviderEmailCommandHandler : IRequestHandler<ProviderEmailCommand>
    {
        private readonly IProviderAccountApiClient<ProviderAccountApiConfiguration> _providerAccountApiClient;

        public ProviderEmailCommandHandler(IProviderAccountApiClient<ProviderAccountApiConfiguration> providerAccountApiClient)
        {
            _providerAccountApiClient = providerAccountApiClient;
        }

        public async Task<Unit> Handle(ProviderEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await _providerAccountApiClient.PostWithResponseCode<object>(
               new PostSendProviderEmailsRequest(request.ProviderId, request.ProviderEmailRequest), false);

            result.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
