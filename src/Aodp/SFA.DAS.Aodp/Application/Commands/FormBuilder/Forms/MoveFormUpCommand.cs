using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class MoveFormUpCommand : IRequest<BaseMediatrResponse<MoveFormUpCommandResponse>>
{
    public readonly Guid FormId;

    public MoveFormUpCommand(Guid formVersionId)
    {
        FormId = formVersionId;
    }
}
