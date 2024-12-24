using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

public class DeclineAcceptedFundingRequest(int applicationId, DeclineAcceptedFundingRequestData data) : IPostApiRequest
{
    public int ApplicationId { get; } = applicationId;
    public string PostUrl => $"/applications/{ApplicationId}/decline-accepted-funding";
    public object Data { get; set; } = data;
}

public class DeclineAcceptedFundingRequestData
{
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public int ApplicationId { get; set; }
}
