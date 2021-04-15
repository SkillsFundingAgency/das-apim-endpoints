using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeLoginApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin
{
    public class ApprenticeLoginService
    {
        private readonly IInternalApiClient<ApprenticeLoginConfiguration> _client;
        private readonly ApprenticeLoginConfiguration _configuration;

        public ApprenticeLoginService(IInternalApiClient<ApprenticeLoginConfiguration> client, ApprenticeLoginConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client);

        public async Task SendInvitation(SendInvitationModel invitation)
        {
            await _client.Post<SendInvitationResponse>(
                new SendInvitationRequest(_configuration.IdentityServerClientId)
                {
                    Data = new SendInvitationRequestData
                    {
                        SourceId = invitation.SourceId,
                        Email = invitation.Email,
                        GivenName = invitation.GivenName,
                        FamilyName = invitation.FamilyName,
                        OrganisationName = invitation.OrganisationName,
                        ApprenticeshipName = invitation.ApprenticeshipName,
                        Callback = _configuration.CallbackUrl,
                        UserRedirect = _configuration.RedirectUrl
                    }
                });
        }
    }
}