using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;

public class UpdateSectionCommand : IRequest<BaseMediatrResponse<UpdateSectionCommandResponse>>
{
    public Guid FormVersionId { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
}
