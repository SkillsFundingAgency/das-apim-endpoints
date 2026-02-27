using MediatR;
using SFA.DAS.Aodp.Application.Commands.Qualifications;

namespace SFA.DAS.AODP.Application.Commands.Qualifications;

public class BulkUpdateQualificationStatusCommand : IRequest<BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse>>
{
    public List<Guid> QualificationIds { get; init; } = new();
    public Guid ProcessStatusId { get; init; }
    public string? Comment { get; init; }
    public string? UserDisplayName { get; init; }
}
