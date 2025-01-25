using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class UpdatePageCommand : IRequest<UpdatePageCommandResponse>
{
    public Guid Id { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

