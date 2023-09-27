using MediatR;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;

public class GetMemberProfileWithPreferencesQueryHandler : IRequestHandler<GetMemberProfileWithPreferencesQuery, GetMemberProfileWithPreferencesQueryResult>
{
    private readonly IAanHubRestApiClient _apiClient;
    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
    private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;

    public GetMemberProfileWithPreferencesQueryHandler(
        IAanHubRestApiClient apiClient,
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient)
    {
        _apiClient = apiClient;
        _coursesApiClient = coursesApiClient;
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
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
        result.RegionId = outputMember.RegionId;
        result.UserType = Enum.Parse<MemberUserType>(outputMember.UserType);

        result.IsRegionalChair = outputMember.IsRegionalChair;


        var outputRegions = responseRegionsTask.Result;
        if (outputRegions.Regions.Any()) result.RegionName = outputRegions.Regions.Find(x => x.Id == result.RegionId)!.Area!;

        result.Apprenticeship = new();

        if (outputMember.ApprenticeId.HasValue)
        {
            GetMyApprenticeshipQueryHandler handler = new(_apprenticeAccountsApiClient, _coursesApiClient);
            var responseMyApprenticeship = await handler.Handle(new GetMyApprenticeshipQuery { ApprenticeId = (Guid)outputMember.ApprenticeId }, cancellationToken);

            if (responseMyApprenticeship != null)
            {
                result.Apprenticeship.Level = responseMyApprenticeship!.TrainingCourse!.Level.ToString();
                result.Apprenticeship.Sector = responseMyApprenticeship.TrainingCourse!.Sector!;
                result.Apprenticeship.Programmes = responseMyApprenticeship.TrainingCourse!.Name!;
            }
        }
        return result;
    }
}
