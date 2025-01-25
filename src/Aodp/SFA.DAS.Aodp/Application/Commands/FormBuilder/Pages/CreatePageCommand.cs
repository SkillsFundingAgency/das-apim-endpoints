using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;

public class CreatePageCommand : IRequest<CreatePageCommandResponse>
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

}
