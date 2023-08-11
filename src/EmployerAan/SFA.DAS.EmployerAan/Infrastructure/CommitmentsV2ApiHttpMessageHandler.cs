using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmployerAan.Configuration;

namespace SFA.DAS.EmployerAan.Infrastructure;

[ExcludeFromCodeCoverage]
public class CommitmentsV2ApiHttpMessageHandler : DelegatingHandler
{
    private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
    private readonly CommitmentsV2ApiConfiguration _commitmentsV2ApiConfiguration;

    public CommitmentsV2ApiHttpMessageHandler(IAzureClientCredentialHelper azureClientCredentialHelper, CommitmentsV2ApiConfiguration commitmentsV2ApiConfiguration)
    {
        _azureClientCredentialHelper = azureClientCredentialHelper;
        _commitmentsV2ApiConfiguration = commitmentsV2ApiConfiguration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Version", "1.0");
        if (!string.IsNullOrEmpty(_commitmentsV2ApiConfiguration.Identifier))
        {
            var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_commitmentsV2ApiConfiguration.Identifier);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}