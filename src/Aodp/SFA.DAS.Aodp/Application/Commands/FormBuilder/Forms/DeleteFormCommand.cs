using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class DeleteFormCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    public readonly Guid FormId;

    public DeleteFormCommand(Guid formId)
    {
        FormId = formId;
    }
}