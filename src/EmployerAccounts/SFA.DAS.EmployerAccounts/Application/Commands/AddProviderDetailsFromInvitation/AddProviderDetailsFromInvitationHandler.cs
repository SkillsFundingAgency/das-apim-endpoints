using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Commands.AddProviderDetailsFromInvitation
{
    public class AddProviderDetailsFromInvitationHandler : IRequestHandler<AddProviderDetailsFromInvitationCommand, Unit>
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _providerRelationshipsApiClient;
        private readonly ILogger<AddProviderDetailsFromInvitationHandler> _logger;

        public AddProviderDetailsFromInvitationHandler(
            ILogger<AddProviderDetailsFromInvitationHandler> logger,
            IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient)
        {
            _logger = logger;
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }

        public async Task<Unit> Handle(AddProviderDetailsFromInvitationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Adding Provider Relationship from Invitation for {request.AccountId}");

            var result = await _providerRelationshipsApiClient
                .PostWithResponseCode<AddAccountProviderFromInvitationResponse>(
                new PostAddProviderDetailsFromInvitationRequest(request.AccountId, request.CorrelationId, request.UserId)
                );

            result.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}