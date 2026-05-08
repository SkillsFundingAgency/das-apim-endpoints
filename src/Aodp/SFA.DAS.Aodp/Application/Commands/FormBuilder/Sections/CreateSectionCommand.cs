using MediatR;
using SFA.DAS.Aodp.Validation;
namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class CreateSectionCommand : IRequest<BaseMediatrResponse<CreateSectionCommandResponse>>
{
    public Guid FormVersionId { get; set; }

    [AllowedCharacters(TextCharacterProfile.Title)]
    public string Title { get; set; }

}