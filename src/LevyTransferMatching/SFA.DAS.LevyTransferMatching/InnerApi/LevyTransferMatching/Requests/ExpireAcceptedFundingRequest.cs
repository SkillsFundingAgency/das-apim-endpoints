using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

public class ExpireAcceptedFundingRequest(int applicationId, ExpireAcceptedFundingRequestData data) : IPostApiRequest
{
    public int ApplicationId { get; } = applicationId;
    public string PostUrl => $"/applications/{ApplicationId}/expire-accepted-funding";
    public object Data { get; set; } = data;
}

public class ExpireAcceptedFundingRequestData
{
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public int ApplicationId { get; set; }
}