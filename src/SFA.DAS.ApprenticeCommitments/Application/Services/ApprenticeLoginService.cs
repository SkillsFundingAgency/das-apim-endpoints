using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Apis;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeLoginApi;

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

        public async Task SendInvitation(Guid guid, string email)
        {
            await _client.Post<SendInvitationResponse>(new SendInvitationRequest
            {
                ClientId = _configuration.ClientId,
                Data = new SendInvitationRequestData
                {
                    SourceId  = guid,
                    Email = email,
                    Callback = _configuration.CallbackUrl,
                    UserRedirect = _configuration.RedirectUrl
                }
            });
        }
    }
}