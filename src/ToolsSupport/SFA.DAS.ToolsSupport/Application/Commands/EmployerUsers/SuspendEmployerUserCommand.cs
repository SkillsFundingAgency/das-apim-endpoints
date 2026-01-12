using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Commands.EmployerUsers;

public record SuspendEmployerUserCommand(string Identifier, string ChangedByUserId, string ChangedByEmail)
    : IRequest<ApiResponse<ChangeUserStatusResponse>>;

public class SuspendEmployerUserCommandHandler(
    IInternalApiClient<EmployerProfilesApiConfiguration> employerProfilesApiClient)
    : IRequestHandler<SuspendEmployerUserCommand, ApiResponse<ChangeUserStatusResponse>>
{
    public Task<ApiResponse<ChangeUserStatusResponse>> Handle(
        SuspendEmployerUserCommand request,
        CancellationToken cancellationToken)
    {
        var data = new ChangeUserStatusRequestData
        {
            ChangedByEmail = request.ChangedByEmail,
            ChangedByUserId = request.ChangedByUserId
        };

        return employerProfilesApiClient.PostWithResponseCode<ChangeUserStatusResponse>(
            new SuspendEmployerUserRequest(request.Identifier, data));
    }
}

