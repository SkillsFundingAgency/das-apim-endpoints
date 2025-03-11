using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Infrastructure;

public class AccessTokenApiClient<T> : ApiClient<T>, IAccessTokenApiClient<T> where T : IAccessTokenApiConfiguration
{
    private readonly HttpClient _tokenClient;
    private readonly ILogger<AccessTokenApiClient<T>> _logger;

    public AccessTokenApiClient(
        ILogger<AccessTokenApiClient<T>> logger,
        IHttpClientFactory httpClientFactory,
        T apiConfiguration) : base(httpClientFactory, apiConfiguration)
    {
        _logger = logger;
        _tokenClient = httpClientFactory.CreateClient();
        _tokenClient.BaseAddress = new Uri(apiConfiguration.TokenSettings.Url);
    }

    protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        if (Configuration.TokenSettings.ShouldSkipForLocal)
        {
            _logger.LogWarning("Token acquisition is skipped. This should not happen in a production environment.");
            return;
        }

        var token = string.Empty;

        try
        {
            token = await GetAccessToken();
        }
        catch (Exception e)
        {
            throw new UnauthorizedAccessException("Could not retrieve access token", e);
        }
            
        httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");
    }

    private async Task<string> GetAccessToken()
    {
        var tokenMessage = new HttpRequestMessage(HttpMethod.Get, Configuration.TokenSettings.Tenant);
        tokenMessage.Content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("client_id", Configuration.TokenSettings.ClientId),
            new KeyValuePair<string, string>("scope", Configuration.TokenSettings.Scope),
            new KeyValuePair<string, string>("client_secret", Configuration.TokenSettings.ClientSecret),
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        ]);

        var response = await _tokenClient.SendAsync(tokenMessage).ConfigureAwait(false);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Could not retrieve access token. Status code: {response.StatusCode}, Response: {json}");
        }

        var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(json);

        return tokenResponse.access_token;
    }

    private class TokenResponse
    {
        public string access_token { get; set; }
    }
}