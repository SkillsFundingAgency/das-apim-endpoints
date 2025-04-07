using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;

public class CreateFormVersionCommand : IRequest<BaseMediatrResponse<CreateFormVersionCommandResponse>>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
}
