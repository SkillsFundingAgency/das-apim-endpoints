using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetUserByUserRef;

public class GetUserByUserRefQuery : IRequest<GetUserByUserRefQueryResult>
{
    public string UserRef { get; set; } = "";
}