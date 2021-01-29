using SFA.DAS.ApprenticeCommitments.Apis;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeLoginApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
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

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _client.GetResponseCode(new GetHealthRequest());
                return status == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task SendInvitation(SendInvitationModel invitation)
        {
            await _client.Post<SendInvitationResponse>(new SendInvitationRequest
            {
                ClientId = _configuration.IdentityServerClientId,
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

    public class SendInvitationModel
    {
        public Guid SourceId { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string OrganisationName { get; set; }
        public string ApprenticeshipName { get; set; }
    }
}