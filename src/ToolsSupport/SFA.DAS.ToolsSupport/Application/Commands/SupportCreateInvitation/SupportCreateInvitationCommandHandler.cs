using System.Net;
using MediatR;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Commands.SupportCreateInvitation;

public class SupportCreateInvitationCommandHandler(IAccountsService accountsService) : IRequestHandler<SupportCreateInvitationCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(SupportCreateInvitationCommand command, CancellationToken cancellationToken)
    {
        var data = new SendInvitationRequestData
        {
            HashedAccountId = command.HashedAccountId,
            NameOfPersonBeingInvited = command.FullName,
            EmailOfPersonBeingInvited = command.Email,
            RoleOfPersonBeingInvited = command.Role
        };

        var response = await accountsService.SendInvitation(new SendInvitationRequest(data));
        return response.StatusCode;
    }
}   