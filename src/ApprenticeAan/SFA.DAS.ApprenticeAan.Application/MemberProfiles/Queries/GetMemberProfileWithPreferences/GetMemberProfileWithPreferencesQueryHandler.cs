using MediatR;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
public class GetMemberProfileWithPreferencesQueryHandler : IRequestHandler<GetMemberProfileWithPreferencesQuery, GetMemberProfileWithPreferencesQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetMemberProfileWithPreferencesQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetMemberProfileWithPreferencesQueryResult?> Handle(GetMemberProfileWithPreferencesQuery request, CancellationToken cancellationToken)
    {
        var responseMemberProfileWithPreferenceTask = _apiClient.GetMemberProfileWithPreferences(request.MemberId, request.RequestedByMemberId, request.IsPublicView, cancellationToken);
        var responseMember = _apiClient.GetMember(request.MemberId, cancellationToken);

        await Task.WhenAll(responseMemberProfileWithPreferenceTask, responseMember);

        GetMemberProfileWithPreferencesQueryResult result = new();

        var outputMemberProfileWithPreferenceTask = responseMemberProfileWithPreferenceTask.Result;

        if (outputMemberProfileWithPreferenceTask.Profiles.Any())
            result.Profiles = outputMemberProfileWithPreferenceTask.Profiles;

        if (outputMemberProfileWithPreferenceTask.Preferences.Any())
            result.Preferences = outputMemberProfileWithPreferenceTask.Preferences;

        var outputMember = responseMember.Result;

        result.FullName = outputMember.FullName;
        result.Email = outputMember.Email;
        result.RegionId = outputMember.RegionId;
        result.UserType = Enum.Parse<MemberUserType>(outputMember.UserType);

        ///if (outputMember.IsRegionalChair.HasValue)
        result.IsRegionalChair = outputMember.IsRegionalChair;

        ///need to retrieve from respective API
        result.RegionName = null!;
        result.Apprenticeship = null!;

        return result;
    }
}
