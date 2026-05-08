using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class UpdatePageCommand : IRequest<BaseMediatrResponse<UpdatePageCommandResponse>>
{
    public Guid Id { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    [AllowedCharacters(TextCharacterProfile.Title)]
    public string Title { get; set; }
}

