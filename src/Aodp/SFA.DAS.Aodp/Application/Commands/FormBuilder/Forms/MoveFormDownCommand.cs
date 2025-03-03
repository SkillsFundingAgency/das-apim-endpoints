using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class MoveFormDownCommand : IRequest<BaseMediatrResponse<MoveFormDownCommandResponse>>
{
    public readonly Guid FormId;

    public MoveFormDownCommand(Guid formVersionId)
    {
        FormId = formVersionId;
    }
}
