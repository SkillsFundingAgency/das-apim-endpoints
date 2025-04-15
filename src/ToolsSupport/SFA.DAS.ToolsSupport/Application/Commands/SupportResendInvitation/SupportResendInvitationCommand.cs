using MediatR;
using System.Net;

namespace SFA.DAS.ToolsSupport.Application.Commands.SupportResendInvitation;

public class SupportResendInvitationCommand : IRequest<HttpStatusCode>
{
    public string Email { get; set; } = "";
    public string HashedAccountId { get; set; } = "";
}
