using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRegistrations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRegistrations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Commands.AddProviderDetailsFromInvitation
{
    public class AddProviderDetailsFromInvitationHandler : IRequestHandler<AddProviderDetailsFromInvitationCommand, Unit>
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _providerRelationshipsApiClient;
        private readonly IProviderRegistrationsApiClient<ProviderRegistrationsApiConfiguration> _providerRegistrationsApiClient;
        private readonly ILogger<AddProviderDetailsFromInvitationHandler> _logger;

        public AddProviderDetailsFromInvitationHandler(
            ILogger<AddProviderDetailsFromInvitationHandler> logger,
            IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient,
            IProviderRegistrationsApiClient<ProviderRegistrationsApiConfiguration> providerRegistrationsApiClient)
        {
            _logger = logger;
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
            _providerRegistrationsApiClient = providerRegistrationsApiClient;
        }

        public async Task<Unit> Handle(AddProviderDetailsFromInvitationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting Invitation with CorrelationId {correlationId}", request.CorrelationId);

            var invitationResponse = await _providerRegistrationsApiClient.GetWithResponseCode<GetInvitationResponse>(new GetInvitationRequest(request.CorrelationId));

            invitationResponse.EnsureSuccessStatusCode();

            var invitation = invitationResponse.Body;
            if (invitation != null)
            {
                _logger.LogInformation("Adding Provider Relationship from Invitation for {accountId}", request.AccountId);

                await _providerRelationshipsApiClient
                    .PostWithResponseCode<AddAccountProviderFromInvitationResponse>(
                        new PostAddProviderDetailsFromInvitationRequest(request.AccountId,
                            invitation.Ukprn, request.CorrelationId, request.UserId,
                            request.Email, request.FirstName, request.LastName)
                    );
            }

            return Unit.Value;
        }
    }
}