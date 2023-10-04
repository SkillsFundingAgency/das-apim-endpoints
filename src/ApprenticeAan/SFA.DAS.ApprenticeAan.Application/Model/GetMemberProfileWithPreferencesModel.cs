using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Application.Model;

public class GetMemberProfileWithPreferencesModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? OrganisationName { get; set; }
    public int RegionId { get; set; }
    public string RegionName { get; set; }
    public MemberUserType UserType { get; set; }
    public bool IsRegionalChair { get; set; }
    public Apprenticeship? Apprenticeship { get; set; }
    public IEnumerable<MemberProfile> Profiles { get; set; }
    public IEnumerable<MemberPreference> Preferences { get; set; }

    public GetMemberProfileWithPreferencesModel(GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferences, MyApprenticeship myApprenticeship)
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

        if (myApprenticeship.TrainingCourse != null)
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
public class Apprenticeship
{
    public string Sector { get; set; } = null!;
    public string Programme { get; set; } = null!;
    public string Level { get; set; } = null!;
}
