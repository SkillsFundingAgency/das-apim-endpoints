using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;

public class UnpublishFormVersionCommand : IRequest<UnpublishFormVersionCommandResponse>
{
    public readonly Guid FormVersionId;

    public UnpublishFormVersionCommand(Guid formVersionId)
    {
        FormVersionId = formVersionId;
    }
}
