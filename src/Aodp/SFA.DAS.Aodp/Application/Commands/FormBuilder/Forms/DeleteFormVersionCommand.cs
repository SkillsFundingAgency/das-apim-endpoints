using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class DeleteFormVersionCommand : IRequest<BaseMediatrResponse<DeleteFormVersionCommandResponse>>
{
    public readonly Guid FormVersionId;

    public DeleteFormVersionCommand(Guid formVersionId)
    {
        FormVersionId = formVersionId;
    }
}