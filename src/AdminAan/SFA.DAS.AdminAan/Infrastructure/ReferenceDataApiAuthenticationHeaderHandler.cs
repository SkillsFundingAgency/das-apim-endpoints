using SFA.DAS.AdminAan.Configuration;
using SFA.DAS.Api.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace SFA.DAS.AdminAan.Infrastructure;

[ExcludeFromCodeCoverage]
public class ReferenceDataApiAuthenticationHeaderHandler : DelegatingHandler
{
    private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
    private readonly ReferenceDataApi _referenceDataApi;

    public ReferenceDataApiAuthenticationHeaderHandler(IAzureClientCredentialHelper azureClientCredentialHelper, ReferenceDataApi referenceDataApi)
    {
        _azureClientCredentialHelper = azureClientCredentialHelper;
        _referenceDataApi = referenceDataApi;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Version", "1.0");
        if (!string.IsNullOrEmpty(_referenceDataApi.IdentifierUri))
        {
            var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_referenceDataApi.IdentifierUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}