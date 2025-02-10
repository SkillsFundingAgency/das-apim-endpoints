using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

public class DeclineApprovedFundingRequest(int applicationId, DeclineApprovedFundingRequestData data) : IPostApiRequest
{
    public int ApplicationId { get; } = applicationId;
    public string PostUrl => $"/applications/{ApplicationId}/decline-approved-funding";
    public object Data { get; set; } = data;
}

public class DeclineApprovedFundingRequestData
{
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public int ApplicationId { get; set; }
}
