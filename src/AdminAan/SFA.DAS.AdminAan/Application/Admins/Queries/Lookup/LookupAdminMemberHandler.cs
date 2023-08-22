using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;
public class LookupAdminMemberHandler : IRequestHandler<LookupAdminMemberRequest, LookupAdminMemberResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public LookupAdminMemberHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<LookupAdminMemberResult?> Handle(LookupAdminMemberRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetMemberByEmail(request.Email, cancellationToken);

        return result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK
            ? result.GetContent()
            : null;
    }
}
