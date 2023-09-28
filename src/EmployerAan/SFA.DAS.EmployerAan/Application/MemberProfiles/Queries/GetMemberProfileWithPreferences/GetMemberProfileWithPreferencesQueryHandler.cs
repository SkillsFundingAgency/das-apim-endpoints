using MediatR;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;

public class GetMemberProfileWithPreferencesQueryHandler : IRequestHandler<GetMemberProfileWithPreferencesQuery, GetMemberProfileWithPreferencesQueryResult>
{
    private readonly IAanHubRestApiClient _apiClient;
    private readonly ICommitmentsV2ApiClient _commitmentsV2ApiClient;

    public GetMemberProfileWithPreferencesQueryHandler(IAanHubRestApiClient apiClient, ICommitmentsV2ApiClient commitmentsV2ApiClient)
    {
        _apiClient = apiClient;
        _commitmentsV2ApiClient = commitmentsV2ApiClient;
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

        result.Apprenticeship = new();

        GetEmployerMemberSummaryQueryHandler handler = new(_commitmentsV2ApiClient);
        var responseMyApprenticeship = await handler.Handle(new GetEmployerMemberSummaryQuery(outputMember.Employer!.AccountId), cancellationToken);

        if (responseMyApprenticeship != null)
        {
            result.Apprenticeship.ActiveApprenticesCount = responseMyApprenticeship.ActiveCount;
            result.Apprenticeship.Sectors = responseMyApprenticeship.Sectors;
        }

        return result;
    }
}
