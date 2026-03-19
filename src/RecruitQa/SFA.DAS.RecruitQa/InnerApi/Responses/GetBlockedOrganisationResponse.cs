namespace SFA.DAS.RecruitQa.InnerApi.Responses;

public class GetBlockedOrganisationResponse
{
    public Guid Id { get; set; }
    public string OrganisationId  { get; set; }
    public string OrganisationType  { get; set; }
    public string BlockedStatus { get; set; }
    public string Reason { get; set; }
    public string UpdatedByUserId { get; set; }
    public string UpdatedByUserEmail { get; set; }
    public DateTime UpdatedDate { get; set; }
}