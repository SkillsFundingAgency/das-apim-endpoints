using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
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

