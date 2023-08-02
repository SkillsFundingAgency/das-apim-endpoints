using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmployerAan.Configuration;

namespace SFA.DAS.EmployerAan.Infrastructure;

[ExcludeFromCodeCoverage]
public class AanHubApiHttpMessageHandler : DelegatingHandler
{
    private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
    private readonly AanHubApiConfiguration _aanHubApiConfiguration;

    public AanHubApiHttpMessageHandler(IAzureClientCredentialHelper azureClientCredentialHelper, AanHubApiConfiguration aanHubApiConfiguration)
    {
        _azureClientCredentialHelper = azureClientCredentialHelper;
        _aanHubApiConfiguration = aanHubApiConfiguration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Version", "1.0");
        if (!string.IsNullOrEmpty(_aanHubApiConfiguration.Identifier))
        {
            var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_aanHubApiConfiguration.Identifier);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}
