namespace SFA.DAS.Approvals.InnerApi.Requests;

public class CreateEmptyCohortRequest
{
    public UserInfo UserInfo { get; set; }
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public long ProviderId { get; set; }
}
