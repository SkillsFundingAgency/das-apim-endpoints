using System.Net.Http.Headers;
using Azure.Core;
using Azure.Identity;
using SFA.DAS.ToolsSupport.Configuration;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Services;

public class SecureTokenHttpClient(TokenServiceApiConfiguration configuration) : ISecureTokenHttpClient
{
    public async Task<string> GetAsync(string url)
    {
        var accessToken = await GetManagedIdentityAuthenticationResult(configuration.IdentifierUri);

        using var client = new HttpClient();

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await client.SendAsync(httpRequest);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    private static async Task<string> GetManagedIdentityAuthenticationResult(string resource)
    {
        var tokenCredential = new DefaultAzureCredential();
        var accessToken = await tokenCredential.GetTokenAsync(
            new TokenRequestContext(scopes: [resource + "/.default"]) { }
        );
        return accessToken.Token;
    }
}
