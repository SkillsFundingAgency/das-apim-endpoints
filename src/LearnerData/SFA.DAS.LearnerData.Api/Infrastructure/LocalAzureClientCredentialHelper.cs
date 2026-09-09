using SFA.DAS.Api.Common.Interfaces;

namespace SFA.DAS.LearnerData.Api.Infrastructure;

public class LocalAzureClientCredentialHelper : IAzureClientCredentialHelper
{
    public Task<string> GetAccessTokenAsync(string identifier)
    {
        return Task.FromResult(string.Empty);
    }
}
