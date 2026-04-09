using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.Authentication;
using SFA.DAS.SharedOuterApi.Infrastructure.DfeSignIn;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net.Http;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class DfeUsersApiClient
        : DfeSignInApiClient<DfeSignInApiConfiguration>,
          IDfeUsersApiClient<DfeSignInApiConfiguration>
    {
        public DfeUsersApiClient(
            IHttpClientFactory httpClientFactory,
            DfeSignInApiConfiguration apiConfiguration,
            IDfeJwtProvider jwtProvider)
            : base(httpClientFactory, apiConfiguration, jwtProvider)
        {
        }
    }
}
