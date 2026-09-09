using System.Net.Http.Headers;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Apim.Shared.Infrastructure;

namespace SFA.DAS.Aodp.Services;

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