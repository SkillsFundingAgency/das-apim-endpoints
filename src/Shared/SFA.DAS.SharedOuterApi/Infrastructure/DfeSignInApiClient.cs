using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Infrastructure.DfeSignIn
{
    public abstract class DfeSignInApiClient<TConfig> : ApiClient<TConfig>
        where TConfig : DfeSignInApiConfiguration
    {
        private readonly IDfeJwtProvider _jwtProvider;

        protected DfeSignInApiClient(
            IHttpClientFactory httpClientFactory,
            TConfig apiConfiguration,
            IDfeJwtProvider jwtProvider)
            : base(httpClientFactory, apiConfiguration)
        {
            _jwtProvider = jwtProvider;
        }

        protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            var token = _jwtProvider.CreateToken();
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return Task.CompletedTask;
        }
    }
}
