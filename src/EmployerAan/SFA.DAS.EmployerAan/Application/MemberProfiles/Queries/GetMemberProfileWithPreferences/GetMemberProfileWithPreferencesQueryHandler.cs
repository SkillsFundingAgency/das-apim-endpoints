using MediatR;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;

public class GetMemberProfileWithPreferencesQueryHandler : IRequestHandler<GetMemberProfileWithPreferencesQuery, GetMemberProfileWithPreferencesQueryResult>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetMemberProfileWithPreferencesQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetMemberProfileWithPreferencesQueryResult> Handle(GetMemberProfileWithPreferencesQuery request, CancellationToken cancellationToken)
    {
        var responseMemberProfileWithPreferenceTask = _apiClient.GetMemberProfileWithPreferences(request.MemberId, request.RequestedByMemberId, request.IsPublicView, cancellationToken);
        var responseMemberTask = _apiClient.GetMember(request.MemberId, cancellationToken);
        var responseRegionsTask = _apiClient.GetRegions(cancellationToken);

        await Task.WhenAll(responseMemberProfileWithPreferenceTask, responseMemberTask, responseRegionsTask);

        GetMemberProfileWithPreferencesQueryResult result = new();

        var outputMemberProfileWithPreferenceTask = responseMemberProfileWithPreferenceTask.Result;

        if (outputMemberProfileWithPreferenceTask.Profiles.Any())
            result.Profiles = outputMemberProfileWithPreferenceTask.Profiles;

        if (outputMemberProfileWithPreferenceTask.Preferences.Any())
            result.Preferences = outputMemberProfileWithPreferenceTask.Preferences;

        var outputMember = responseMemberTask.Result;

        result.FullName = outputMember.FullName;
        result.FirstName = outputMember.FirstName;
        result.LastName = outputMember.LastName;
        result.OrganisationName = outputMember.OrganisationName;
        result.Email = outputMember.Email;
        result.RegionId = (int)outputMember.RegionId!;
        result.UserType = Enum.Parse<MemberUserType>(outputMember.UserType);

        result.IsRegionalChair = outputMember.IsRegionalChair;

        var outputRegions = responseRegionsTask.Result;
        if (outputRegions.Regions.Any()) result.RegionName = outputRegions.Regions.Find(x => x.Id == result.RegionId)!.Area!;

        result.AccountId = outputMember.Employer!.AccountId;

        return result;
    }
}
