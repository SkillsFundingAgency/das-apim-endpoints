using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Services;

public interface IRecruitArtificialIntelligenceClient
{
    Task SendPayloadAsync(object payload, CancellationToken cancellationToken);
}

public class RecruitArtificialIntelligenceClient(
    IInternalApiClient<RecruitAiApiConfiguration> internalApiClient) : IRecruitArtificialIntelligenceClient
{
    private sealed class PostRequest(string url, object data): IPostApiRequest
    {
        public string PostUrl => url;
        public object Data { get; set; } = data;
    }
    
    public async Task SendPayloadAsync(object payload, CancellationToken cancellationToken)
    {
        var request = new PostRequest("api/llm", payload);
        await internalApiClient.PostWithResponseCode<NullResponse>(request, includeResponse: false);
    }
}


