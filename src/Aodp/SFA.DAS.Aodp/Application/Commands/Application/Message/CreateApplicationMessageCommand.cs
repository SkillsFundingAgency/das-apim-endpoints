using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Application.Message;

public class CreateApplicationMessageCommand : IRequest<BaseMediatrResponse<CreateApplicationMessageCommandResponse>>
{
    public Guid? ApplicationId { get; set; }
    public string MessageText { get; set; }
    public string MessageType { get; set; }
    public string UserType { get; set; }
    public string SentByName { get; set; }
    public string SentByEmail { get; set; }
}