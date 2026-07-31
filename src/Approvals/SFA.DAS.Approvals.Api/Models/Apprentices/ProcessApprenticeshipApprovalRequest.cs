using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class ProcessApprenticeshipApprovalRequest
{
    public bool ApplyChanges { get; set; }
    public long AccountId { get; set; }
    public UserInfo UserInfo { get; set; }
}
