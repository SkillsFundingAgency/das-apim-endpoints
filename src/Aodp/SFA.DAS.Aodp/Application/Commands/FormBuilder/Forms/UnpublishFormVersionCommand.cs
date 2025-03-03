using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class UnpublishFormVersionCommand : IRequest<BaseMediatrResponse<UnpublishFormVersionCommandResponse>>
{
    public readonly Guid FormVersionId;

    public UnpublishFormVersionCommand(Guid formVersionId)
    {
        FormVersionId = formVersionId;
    }
}
