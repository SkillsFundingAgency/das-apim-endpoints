namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class BlockedOrganisationDto
{
    public required string OrganisationId { get; set; }
    public required string OrganisationType { get; set; }
    public required string BlockedStatus { get; set; }
    public required string Reason { get; set; }
    public required string UpdatedByUserId { get; set; }
    public required string UpdatedByUserEmail { get; set; }
}