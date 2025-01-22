using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class DeletePageCommand : IRequest<DeletePageCommandResponse>
{
    public readonly Guid PageId;

    public DeletePageCommand(Guid pageId)
    {
        PageId = pageId;
    }
}