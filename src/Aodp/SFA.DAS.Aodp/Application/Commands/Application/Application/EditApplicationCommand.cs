using MediatR;
using SFA.DAS.Aodp.Validation;

public class EditApplicationCommand : IRequest<BaseMediatrResponse<EditApplicationCommandResponse>>
{
    [QualificationNumber]
    public string? QualificationNumber { get; set; }
    [AllowedCharacters(TextCharacterProfile.Title)]
    public string Title { get; set; }
    [AllowedCharacters(TextCharacterProfile.PersonName)]
    public string Owner { get; set; }
    public Guid ApplicationId { get; set; }
}

