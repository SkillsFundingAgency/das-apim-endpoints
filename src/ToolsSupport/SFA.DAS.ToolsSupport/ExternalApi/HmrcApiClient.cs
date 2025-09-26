using System.Net.Http.Headers;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.ToolsSupport.Configuration;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.ExternalApi;

public class HmrcApiClient<T>(IHttpClientFactory httpClientFactory, T apiConfiguration, ITokenService tokenService) : GetApiClient<T>(httpClientFactory, apiConfiguration), IHmrcApiClient<T> where T : IHmrcApiConfiguration
{
    protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        var tokenResult = await tokenService.GetPrivilegedAccessTokenAsync();
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.AccessCode);
        
        httpRequestMessage.Headers.Accept.Clear();
        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.hmrc.1.0+json"));
    }
}