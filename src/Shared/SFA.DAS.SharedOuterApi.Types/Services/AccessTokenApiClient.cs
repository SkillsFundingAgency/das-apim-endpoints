using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Services;

public class AccessTokenApiClient<T> : ApiClient<T>, IAccessTokenApiClient<T> where T : IAccessTokenApiConfiguration
{
    private static readonly TimeSpan ExpiryMargin = TimeSpan.FromSeconds(60);
    private static readonly ConcurrentDictionary<string, CachedToken> TokenCache = new();

    private readonly HttpClient _tokenClient;
    private readonly ILogger<AccessTokenApiClient<T>> _logger;
    private readonly TimeProvider _timeProvider;

    public AccessTokenApiClient(
        ILogger<AccessTokenApiClient<T>> logger,
        IHttpClientFactory httpClientFactory,
        T apiConfiguration,
        TimeProvider timeProvider) : base(httpClientFactory, apiConfiguration)
    {
        _logger = logger;
        _timeProvider = timeProvider;
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
        var settings = Configuration.TokenSettings;
        var cacheKey = $"{settings.Url}|{settings.Tenant}|{settings.ClientId}|{settings.Scope}";

        if (TokenCache.TryGetValue(cacheKey, out var cached) && cached.ExpiresAt > _timeProvider.GetUtcNow())
        {
            return cached.Token;
        }

        var tokenMessage = new HttpRequestMessage(HttpMethod.Post, Configuration.TokenSettings.Tenant);
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

        var lifetime = TimeSpan.FromSeconds(tokenResponse.expires_in);
        if (lifetime > ExpiryMargin)
        {
            TokenCache[cacheKey] = new CachedToken(tokenResponse.access_token, _timeProvider.GetUtcNow() + lifetime - ExpiryMargin);
        }

        return tokenResponse.access_token;
    }

    private record CachedToken(string Token, DateTimeOffset ExpiresAt);

    private class TokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}