using System.Net;
using MediatR;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.Members.GetMemberProfile;

public class GetMemberProfileQueryHandler : IRequestHandler<GetMemberProfileQuery, GetMemberProfileQueryResult>
{
    private readonly IAanHubRestApiClient _aanHubApiClient;
    private readonly ICommitmentsV2ApiClient _commitmentsV2ApiClient;
    private readonly IApprenticeAccountsApiClient _apprenticeAccountsApiClient;
    private readonly ICoursesApiClient _coursesApiClient;

    public GetMemberProfileQueryHandler(IAanHubRestApiClient aanHubApiClient, ICommitmentsV2ApiClient commitmentsV2ApiClient, IApprenticeAccountsApiClient apprenticeAccountsApiClient, ICoursesApiClient coursesApiClient)
    {
        _aanHubApiClient = aanHubApiClient;
        _commitmentsV2ApiClient = commitmentsV2ApiClient;
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
        _coursesApiClient = coursesApiClient;
    }

    public async Task<GetMemberProfileQueryResult> Handle(GetMemberProfileQuery request, CancellationToken cancellationToken)
    {
        GetMemberProfileQueryResult result = new();

        var getMemberTask = _aanHubApiClient.GetMember(request.MemberId, cancellationToken);
        var getProfileAndPreferencesTask = _aanHubApiClient.GetMemberProfileWithPreferences(request.MemberId, request.RequestedByMemberId, cancellationToken);

        await Task.WhenAll(getMemberTask, getProfileAndPreferencesTask);

        UpdateResultWithMemberDetails(result, getMemberTask.Result, getProfileAndPreferencesTask.Result);

        await UpdateResultWithRegionName(result, getMemberTask.Result.RegionId, cancellationToken);

        if (getMemberTask.Result.UserType == MemberUserType.Employer)
        {
            await UpdateResultWithApprenticeshipDetailsForEmployer(result, getMemberTask.Result.Employer!.AccountId, cancellationToken);
        }
        else
        {
            await UpdateResultWithApprenticeshipDetailsForApprentice(result, getMemberTask.Result.Apprentice!.ApprenticeId, cancellationToken);
        }

        return result;
    }

    private static void UpdateResultWithMemberDetails(GetMemberProfileQueryResult result, GetMemberResponse memberResponse, GetMemberProfilesAndPreferencesResponse profilesAndPreferencesResponse)
    {
        result.FullName = memberResponse.FullName;
        result.FirstName = memberResponse.FirstName;
        result.LastName = memberResponse.LastName;
        result.OrganisationName = memberResponse.OrganisationName;
        result.Email = memberResponse.Email;
        result.RegionId = memberResponse.RegionId;
        result.UserType = memberResponse.UserType;
        result.IsRegionalChair = memberResponse.IsRegionalChair.GetValueOrDefault();
        result.Profiles = profilesAndPreferencesResponse.Profiles;
        result.Preferences = profilesAndPreferencesResponse.Preferences;
    }

    private async Task UpdateResultWithRegionName(GetMemberProfileQueryResult result, int? regionId, CancellationToken cancellationToken)
    {
        if (regionId == null) return;
        var regionsResponse = await _aanHubApiClient.GetRegions(cancellationToken);
        result.RegionName = regionsResponse.Regions.First(r => r.Id == regionId.Value).Area;
    }

    private async Task UpdateResultWithApprenticeshipDetailsForEmployer(GetMemberProfileQueryResult result, long accountId, CancellationToken cancellationToken)
    {
        var accountSummaryTask = _commitmentsV2ApiClient.GetEmployerAccountSummary(accountId, cancellationToken);
        var apprenticeshipSummaryTask = _commitmentsV2ApiClient.GetEmployerApprenticeshipsSummary(accountId, cancellationToken);

        await Task.WhenAll(accountSummaryTask, apprenticeshipSummaryTask);

        result.Apprenticeship = new()
        {
            Sectors = apprenticeshipSummaryTask.Result.Sectors,
            ActiveApprenticesCount = accountSummaryTask.Result.ApprenticeshipStatusSummaryResponse.FirstOrDefault()?.ActiveCount
        };
    }

    private async Task UpdateResultWithApprenticeshipDetailsForApprentice(GetMemberProfileQueryResult result, Guid apprenticeId, CancellationToken cancellationToken)
    {
        result.Apprenticeship = new();

        var myApprenticeshipResponse = await _apprenticeAccountsApiClient.GetMyApprenticeship(apprenticeId, cancellationToken);
        if (myApprenticeshipResponse.ResponseMessage.StatusCode == HttpStatusCode.NotFound) return;

        var myApprenticeship = myApprenticeshipResponse.GetContent();

        var standardUId = myApprenticeship.StandardUId;
        if (standardUId != null)
        {
            var standardResponse = await _coursesApiClient.GetStandard(standardUId!, cancellationToken);
            if (standardResponse.ResponseMessage.IsSuccessStatusCode)
            {
                var standard = standardResponse.GetContent();
                result.Apprenticeship!.Sector = standard.Route;
                result.Apprenticeship!.Level = standard.Level.ToString();
                result.Apprenticeship!.Programme = standard.Title;
            }
        }
        else
        {
            if (myApprenticeship.TrainingCode != null)
            {
                var frameworkResponse = await _coursesApiClient.GetFramework(myApprenticeship.TrainingCode, cancellationToken);
                if (frameworkResponse.ResponseMessage.IsSuccessStatusCode)
                {
                    var framework = frameworkResponse.GetContent();
                    result.Apprenticeship!.Sector = framework.FrameworkName;
                    result.Apprenticeship!.Level = framework.Level.ToString();
                    result.Apprenticeship!.Programme = framework.Title;

                }
            }
        }
    }
}
