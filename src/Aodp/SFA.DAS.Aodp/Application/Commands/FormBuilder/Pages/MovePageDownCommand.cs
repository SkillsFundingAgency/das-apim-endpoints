using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class MovePageDownCommand : IRequest<BaseMediatrResponse<MovePageDownCommandResponse>>
{
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
}

