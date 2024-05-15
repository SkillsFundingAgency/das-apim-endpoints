using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.Admins.Queries.GetAdminMember;
public class GetAdminMemberHandler : IRequestHandler<GetAdminMemberRequest, GetAdminMemberResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetAdminMemberHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetAdminMemberResult?> Handle(GetAdminMemberRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetMemberByEmail(request.Email, cancellationToken);

        return result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK
            ? result.GetContent()
            : null;
    }
}
