using MediatR;

namespace SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;

public class UpdateFormVersionCommand : IRequest<UpdateFormVersionCommandResponse>
{
    public Guid FormVersionId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }

}
