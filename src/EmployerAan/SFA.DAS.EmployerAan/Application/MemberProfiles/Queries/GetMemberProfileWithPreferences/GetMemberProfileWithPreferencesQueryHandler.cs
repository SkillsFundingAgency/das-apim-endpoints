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
        var memberProfileWithPreferenceResponse = _apiClient.GetMemberProfileWithPreferences(request.MemberId, request.RequestedByMemberId, request.IsPublicView, cancellationToken);
        var memberResponse = _apiClient.GetMember(request.MemberId, cancellationToken);
        var regionsResponse = _apiClient.GetRegions(cancellationToken);

        await Task.WhenAll(memberProfileWithPreferenceResponse, memberResponse, regionsResponse);

        GetMemberProfileWithPreferencesQueryResult result = new();

        var outputMemberProfileWithPreferenceTask = memberProfileWithPreferenceResponse.Result;

        if (outputMemberProfileWithPreferenceTask.Profiles.Any())
            result.Profiles = outputMemberProfileWithPreferenceTask.Profiles;

        if (outputMemberProfileWithPreferenceTask.Preferences.Any())
            result.Preferences = outputMemberProfileWithPreferenceTask.Preferences;

        var outputMember = memberResponse.Result;

        result.FullName = outputMember.FullName;
        result.FirstName = outputMember.FirstName;
        result.LastName = outputMember.LastName;
        result.OrganisationName = outputMember.OrganisationName;
        result.Email = outputMember.Email;
        result.RegionId = (int)outputMember.RegionId!;
        result.UserType = Enum.Parse<MemberUserType>(outputMember.UserType);

        result.IsRegionalChair = (bool)outputMember.IsRegionalChair!;

        var outputRegions = regionsResponse.Result;
        if (outputRegions.Regions.Any()) result.RegionName = outputRegions.Regions.Find(x => x.Id == result.RegionId)!.Area!;

        result.AccountId = outputMember.Employer!.AccountId;

        return result;
    }
}
