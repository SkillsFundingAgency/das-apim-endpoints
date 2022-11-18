using Microsoft.AspNetCore.Hosting;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.TrackProgressInternal.Application.Configuration;
using System.Net.Http.Headers;

namespace SFA.DAS.TrackProgressInternal.Application.Services;

public class ApimClient
{
    private readonly HttpClient _httpClient;
    private readonly Func<HttpRequestMessage, Task> _addAuthentication;

    public ApimClient(
        IHttpClientFactory httpClientFactory,
        IOwnerApiConfiguration configuration,
        IWebHostEnvironment hostingEnvironment,
        IAzureClientCredentialHelper azureClientCredentialHelper)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(configuration.Url);

        if (hostingEnvironment.IsLocalOrDev())
            _addAuthentication = _ => Task.CompletedTask;
        else
            _addAuthentication = async request => await AddAuthentication(request);

        async Task AddAuthentication(HttpRequestMessage request)
        {
            var accessToken = await azureClientCredentialHelper.GetAccessTokenAsync(configuration.Identifier);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    internal async Task<HttpResponseMessage> Send(HttpRequestMessage request, string apiVersion = "1.0")
    {
        await _addAuthentication(request);
        AddVersionHeader(request, apiVersion);
        return await _httpClient.SendAsync(request);
    }

    private void AddVersionHeader(HttpRequestMessage request, string requestVersion)
    {
        request.Headers.Remove("X-Version");
        request.Headers.Add("X-Version", requestVersion);
    }
}