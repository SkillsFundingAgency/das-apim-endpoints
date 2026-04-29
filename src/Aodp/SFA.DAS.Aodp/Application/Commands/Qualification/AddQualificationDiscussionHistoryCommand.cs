using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.Qualification;

public class AddQualificationDiscussionHistoryCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    [QualificationNumber]
    public string QualificationReference { get; set; } = string.Empty;

    [AllowedCharacters(TextCharacterProfile.PersonName)]
    public string? UserDisplayName { get; set; }

    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string? Notes { get; set; }
}
