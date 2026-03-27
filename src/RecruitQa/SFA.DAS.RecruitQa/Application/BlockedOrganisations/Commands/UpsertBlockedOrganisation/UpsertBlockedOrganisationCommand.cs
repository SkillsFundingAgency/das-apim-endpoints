using MediatR;

namespace SFA.DAS.RecruitQa.Application.BlockedOrganisations.Commands.UpsertBlockedOrganisation;

public class UpsertBlockedOrganisationCommand : IRequest
{
    public Guid Id { get; set; }
    public required string BlockedStatus { get; set; }
    public required string OrganisationId { get; set; }
    public required string Reason { get; set; }
    public required string OrganisationType { get; set; }
    public required string UpdatedByUserEmail { get; set; }
    public required string UpdatedByUserId { get; set; }
}