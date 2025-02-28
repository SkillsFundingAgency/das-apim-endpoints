using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries;
public class GetUsersByEmailQuery : IRequest<GetUsersByEmailQueryResult>
{
    public string Email { get; set; }
}

