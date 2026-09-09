using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class PostGetLearnersForProviderRequest : IPostApiRequest
{
    public readonly long ProviderId;
    public string PostUrl => $"providers/{ProviderId}/learners";
    public object Data { get; set; }

    public PostGetLearnersForProviderRequest(long providerId, GetLearnersForProviderRequest data)
    {
        ProviderId = providerId;
        Data = data;
    }
}
