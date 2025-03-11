using System.Net;
using MediatR;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Commands.ChangeUserRole;

public class ChangeUserRoleCommandHandler(IAccountsService accountsService) : IRequestHandler<ChangeUserRoleCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(ChangeUserRoleCommand command, CancellationToken cancellationToken)
    {
        var data = new ChangeUserRoleRequestData
        {
            HashedAccountId = command.HashedAccountId,
            Email = command.Email,
            Role = command.Role
        };

        var response = await accountsService.ChangeUserRole(new ChangeUserRoleRequest(data));
        return response.StatusCode;
    }
}