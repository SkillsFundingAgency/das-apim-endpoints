using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.Interfaces;

public interface ISecureTokenHttpClient<T> : IInternalApiClient<T>
{
    //Task<string> GetAsync(string url);
}
