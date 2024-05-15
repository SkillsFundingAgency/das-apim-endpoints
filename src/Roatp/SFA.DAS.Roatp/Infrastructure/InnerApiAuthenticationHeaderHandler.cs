using SFA.DAS.Api.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.Infrastructure;
[ExcludeFromCodeCoverage]
public class InnerApiAuthenticationHeaderHandler : DelegatingHandler
{
    private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
    private readonly string _apiIdentifier;

    public InnerApiAuthenticationHeaderHandler(IAzureClientCredentialHelper azureClientCredentialHelper,
        string apiIdentifier)
    {
        _azureClientCredentialHelper = azureClientCredentialHelper;
        _apiIdentifier = apiIdentifier;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Version", "1.0");
        if (!string.IsNullOrEmpty(_apiIdentifier))
        {
            var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_apiIdentifier);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}
