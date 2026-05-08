using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class UpdateSectionCommand : IRequest<BaseMediatrResponse<UpdateSectionCommandResponse>>
{
    public Guid FormVersionId { get; set; }
    public Guid Id { get; set; }
    [AllowedCharacters(TextCharacterProfile.Title)]
    public string Title { get; set; }
}
