using System.Net;
using MediatR;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Commands.SupportResendInvitation;

public class SupportResendInvitationCommandHandler(IAccountsService accountsService) : IRequestHandler<SupportResendInvitationCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(SupportResendInvitationCommand command, CancellationToken cancellationToken)
    {
        var data = new ResendInvitationRequestData
        {
            HashedAccountId = command.HashedAccountId,
            Email = command.Email
        };

        var response = await accountsService.ResendInvitation(new ResendInvitationRequest(data));
        return response.StatusCode;
    }
}