using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
public class GetMemberProfileWithPreferencesQueryHandler : IRequestHandler<GetMemberProfileWithPreferencesQuery, GetMemberProfileWithPreferencesQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetMemberProfileWithPreferencesQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetMemberProfileWithPreferencesQueryResult?> Handle(GetMemberProfileWithPreferencesQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.GetMemberProfileWithPreferences(request.MemberId, request.IsPublicView, cancellationToken);
    }
}
