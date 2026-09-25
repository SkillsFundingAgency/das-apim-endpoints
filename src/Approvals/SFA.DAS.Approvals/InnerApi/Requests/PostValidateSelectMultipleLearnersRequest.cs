using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class PostValidateSelectMultipleLearnersRequest : IPostApiRequest
{
    public readonly long ProviderId;
    public object Data { get; set; }
    public string PostUrl => $"api/{ProviderId}/selectmultiplelearners/validate";

    public PostValidateSelectMultipleLearnersRequest(long providerId, ValidateSelectMultipleLearnersApiRequest data)
    {
        ProviderId = providerId;
        Data = data;
    }
}