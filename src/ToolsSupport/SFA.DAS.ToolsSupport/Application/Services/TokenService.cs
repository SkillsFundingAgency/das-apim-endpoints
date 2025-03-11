using SFA.DAS.ToolsSupport.Configuration;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Services;

public class TokenService(ISecureTokenHttpClient<TokenServiceApiConfiguration> secureTokenHttpClient) : ITokenService
{
    public async Task<PrivilegedAccessToken> GetPrivilegedAccessTokenAsync()
    {
        return await secureTokenHttpClient.Get<PrivilegedAccessToken>(new GetPrivilegedAccessTokenRequest());
    }
}

public class PrivilegedAccessToken
{
    public string AccessCode { get; set; } = "";

    public DateTime ExpiryTime { get; set; }
}
