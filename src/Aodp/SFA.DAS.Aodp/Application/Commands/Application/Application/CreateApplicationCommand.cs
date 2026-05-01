using MediatR;
using SFA.DAS.Aodp.Validation;

public class CreateApplicationCommand : IRequest<BaseMediatrResponse<CreateApplicationCommandResponse>>
{
    [AllowedCharacters(TextCharacterProfile.Title)]
    public string Title { get; set; }

    [AllowedCharacters(TextCharacterProfile.PersonName)]
    public string Owner { get; set; }

    public Guid FormVersionId { get; set; }

    [QualificationNumber]
    public string? QualificationNumber { get; set; }

    public Guid OrganisationId { get; set; }
    
    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string OrganisationName { get; set; }

    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string OrganisationUkprn { get; set; }
}
