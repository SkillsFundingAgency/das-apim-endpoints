using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;

public class CreatePageCommand : IRequest<BaseMediatrResponse<CreatePageCommandResponse>>
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    public string Title { get; set; }

}
