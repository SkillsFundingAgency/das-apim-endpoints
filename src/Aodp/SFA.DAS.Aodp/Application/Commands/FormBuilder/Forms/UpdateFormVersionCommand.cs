using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class UpdateFormVersionCommand : IRequest<BaseMediatrResponse<UpdateFormVersionCommandResponse>>
{
    public Guid FormVersionId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }

}
