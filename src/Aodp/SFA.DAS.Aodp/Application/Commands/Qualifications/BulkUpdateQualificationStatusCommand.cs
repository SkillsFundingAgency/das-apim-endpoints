using MediatR;
using SFA.DAS.Aodp.Application.Commands.Qualifications;
using SFA.DAS.Aodp.Validation;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AODP.Application.Commands.Qualifications;
[ExcludeFromCodeCoverage]
public class BulkUpdateQualificationStatusCommand : IRequest<BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse>>
{
    public List<Guid> QualificationIds { get; init; } = new();
    public Guid ProcessStatusId { get; init; }

    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string? Comment { get; init; }

    [AllowedCharacters(TextCharacterProfile.PersonName)]
    public string? UserDisplayName { get; init; }
}
