using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;

public class UpdateSectionCommand : IRequest<UpdateSectionCommandResponse>
{
    public Guid FormVersionId { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

}
