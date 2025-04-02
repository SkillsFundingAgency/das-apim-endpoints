using System.Net;
using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Commands.SupportCreateInvitation;

public class SupportCreateInvitationCommand : IRequest<HttpStatusCode>
{
    public string HashedAccountId { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public int Role { get; set; }
}
