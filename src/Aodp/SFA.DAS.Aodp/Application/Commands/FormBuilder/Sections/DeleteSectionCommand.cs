using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;

public class DeleteSectionCommand : IRequest<DeleteSectionCommandResponse>
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
  
}
