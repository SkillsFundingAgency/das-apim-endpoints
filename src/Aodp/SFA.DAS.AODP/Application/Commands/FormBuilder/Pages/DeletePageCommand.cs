using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class DeletePageCommand : IRequest<BaseMediatrResponse<DeletePageCommandResponse>>
{
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

}