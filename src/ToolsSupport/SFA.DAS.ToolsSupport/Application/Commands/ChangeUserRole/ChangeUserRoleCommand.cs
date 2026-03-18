using System.Net;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.ToolsSupport.Application.Commands.ChangeUserRole;

public class ChangeUserRoleCommand : IRequest<HttpStatusCode>
{
    public string HashedAccountId { get; set; } = "";
    public string Email { get; set; } = "";
    public Role Role { get; set; }
}