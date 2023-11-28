using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.EmployerAan.Common;

namespace SFA.DAS.EmployerAan.Models;

public class GetMemberProfileWithPreferencesModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? OrganisationName { get; set; }
    public int? RegionId { get; set; }
    public string RegionName { get; set; }
    public MemberUserType UserType { get; set; }
    public bool IsRegionalChair { get; set; }
    public Apprenticeship Apprenticeship { get; set; } = new();
    public IEnumerable<MemberProfile> Profiles { get; set; }
    public IEnumerable<MemberPreference> Preferences { get; set; }

    public GetMemberProfileWithPreferencesModel(
        GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferences,
        GetMyApprenticeshipQueryResult? myApprenticeship,
        GetEmployerMemberSummaryQueryResult? employerMemberSummaryResult)
    {
        FullName = memberProfileWithPreferences.FullName;
        Email = memberProfileWithPreferences.Email;
        FirstName = memberProfileWithPreferences.FirstName;
        LastName = memberProfileWithPreferences.LastName;
        OrganisationName = memberProfileWithPreferences.OrganisationName;
        RegionId = memberProfileWithPreferences.RegionId;
        RegionName = memberProfileWithPreferences.RegionName;
        UserType = memberProfileWithPreferences.UserType;
        IsRegionalChair = memberProfileWithPreferences.IsRegionalChair;
        Profiles = memberProfileWithPreferences.Profiles;
        Preferences = memberProfileWithPreferences.Preferences;
        if (memberProfileWithPreferences.UserType == MemberUserType.Employer && employerMemberSummaryResult != null)
        {
            Apprenticeship.ActiveApprenticesCount = employerMemberSummaryResult.ActiveCount;
            Apprenticeship.Sectors = employerMemberSummaryResult.Sectors;
        }
        if (memberProfileWithPreferences.UserType == MemberUserType.Apprentice && myApprenticeship != null && myApprenticeship.TrainingCourse != null)
        {
            Apprenticeship = new Apprenticeship
            {
                Level = myApprenticeship.TrainingCourse.Level.ToString(),
                Sector = myApprenticeship.TrainingCourse.Sector!,
                Programme = myApprenticeship.TrainingCourse.Name!
            };
        }
    }
}