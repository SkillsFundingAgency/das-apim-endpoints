using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class CreateFormVersionCommand : IRequest<BaseMediatrResponse<CreateFormVersionCommandResponse>>
{
    [AllowedCharacters(TextCharacterProfile.Title)]
    public string Title { get; set; }

    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string Description { get; set; }
    public int Order { get; set; }
}
