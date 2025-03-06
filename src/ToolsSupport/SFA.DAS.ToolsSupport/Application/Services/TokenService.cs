using Newtonsoft.Json;
using SFA.DAS.ToolsSupport.Configuration;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Services;

public class TokenService(ISecureTokenHttpClient secureTokenHttpClient, TokenServiceApiConfiguration configuration) : ITokenService
{
    public async Task<PrivilegedAccessToken> GetPrivilegedAccessTokenAsync()
    {
        Uri uri = new(new Uri(configuration.ApiBaseUrl), "api/PrivilegedAccess");

        return JsonConvert.DeserializeObject<PrivilegedAccessToken>(await secureTokenHttpClient.GetAsync(uri.ToString()));
    }
}

public class PrivilegedAccessToken
{
    public string AccessCode { get; set; } = "";

    public DateTime ExpiryTime { get; set; }
}
