using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;

public class CreateSectionCommand : IRequest<CreateSectionCommandResponse>
{

    public Guid FormVersionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

}