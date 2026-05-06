using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships;

public class SyncLearnerDataResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public GetDraftApprenticeshipResponse UpdatedDraftApprenticeship { get; set; }
}
