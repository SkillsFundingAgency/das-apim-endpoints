using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Admins.Queries.Lookup;

public class LookupAdminMemberHandler : IRequestHandler<LookupAdminMemberRequestModel, LookupAdminMemberResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public LookupAdminMemberHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<LookupAdminMemberResult?> Handle(LookupAdminMemberRequestModel request,
        CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetMemberByEmail(request.Email, cancellationToken);

        return result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK
            ? new LookupAdminMemberResult { MemberId = result.GetContent().MemberId, Status = result.GetContent().Status }
            : null;
    }
}