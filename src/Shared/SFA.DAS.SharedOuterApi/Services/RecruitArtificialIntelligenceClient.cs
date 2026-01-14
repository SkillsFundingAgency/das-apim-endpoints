using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Services;

public interface IRecruitArtificialIntelligenceClient
{
    Task SendPayloadAsync(object payload, CancellationToken cancellationToken);
}

public class RecruitArtificialIntelligenceClient(ILogger<RecruitArtificialIntelligenceClient> logger,
    IInternalApiClient<RecruitArtificialIntelligenceConfiguration> internalApiClient) : IRecruitArtificialIntelligenceClient
{
    private sealed class PostRequest(string url, object data): IPostApiRequest
    {
        public string PostUrl => url;
        public object Data { get; set; } = data;
    }
    
    public async Task SendPayloadAsync(object payload, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending Ai api request");
        var request = new PostRequest("api/llm", payload);
        var response = await internalApiClient.PostWithResponseCode<NullResponse>(request, includeResponse: false);
        logger.LogInformation("Ai response code was: {ResponseStatusCode}", response.StatusCode);
    }
}


