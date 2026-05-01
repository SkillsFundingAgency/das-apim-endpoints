using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class UpdateFormVersionCommand : IRequest<BaseMediatrResponse<UpdateFormVersionCommandResponse>>
{
    public Guid FormVersionId { get; set; }

    [AllowedCharacters(TextCharacterProfile.Title)]
    public string Name { get; set; }
    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string Description { get; set; }
    public int Order { get; set; }

}
