using System.Net;
using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Commands.ChangeUserRole;

public class ChangeUserRoleCommand : IRequest<HttpStatusCode>
{
    public string HashedAccountId { get; set; } = "";
    public string Email { get; set; } = "";
    public Role Role { get; set; }
}