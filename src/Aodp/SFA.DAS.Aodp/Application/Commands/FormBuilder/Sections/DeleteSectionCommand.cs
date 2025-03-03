using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class DeleteSectionCommand : IRequest<BaseMediatrResponse<DeleteSectionCommandResponse>>
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
  
}
