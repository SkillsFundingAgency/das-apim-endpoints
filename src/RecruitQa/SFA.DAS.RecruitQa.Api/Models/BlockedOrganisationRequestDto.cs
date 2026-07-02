using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.Models;

public class BlockedOrganisationRequestDto
{
    public Guid Id { get; set; }
    public string OrganisationType { get; set; }
    public string OrganisationId { get; set; }
    public string BlockedStatus { get; set; }
    public string UpdatedByUserEmail { get; set; }
    public string UpdatedByUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Reason { get; set; }

    public static implicit operator BlockedOrganisationRequestDto(GetBlockedOrganisationResponse source)
    {
        return new BlockedOrganisationRequestDto
        {
            Id = source.Id,
            OrganisationType = source.OrganisationType,
            OrganisationId = source.OrganisationId,
            BlockedStatus = source.BlockedStatus,
            UpdatedByUserEmail = source.UpdatedByUserEmail,
            UpdatedByUserId = source.UpdatedByUserId,
            UpdatedDate = source.UpdatedDate,
            Reason = source.Reason
        };
    }
}