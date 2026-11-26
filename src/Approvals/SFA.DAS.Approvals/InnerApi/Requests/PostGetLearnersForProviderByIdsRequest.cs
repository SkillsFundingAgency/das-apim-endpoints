using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class PostGetLearnersForProviderByIdsRequest : IPostApiRequest
{
    public readonly long ProviderId;
    public string PostUrl => $"providers/{ProviderId}/learners/by-ids";
    public object Data { get; set; }

    public PostGetLearnersForProviderByIdsRequest(long providerId, GetLearnersForProviderByIdsRequest data)
    {
        ProviderId = providerId;
        Data = data;
    }
}
