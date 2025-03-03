﻿using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Application.Models;

public class GetMemberProfileWithPreferencesModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? OrganisationName { get; set; }
    public int? RegionId { get; set; }
    public string RegionName { get; set; }
    public DateTime JoinedDate { get; set; }
    public MemberUserType UserType { get; set; }
    public bool IsRegionalChair { get; set; }
    public Apprenticeship? Apprenticeship { get; set; }
    public IEnumerable<MemberProfile> Profiles { get; set; }
    public IEnumerable<MemberPreference> Preferences { get; set; }

    public GetMemberProfileWithPreferencesModel(GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferences, GetMyApprenticeshipQueryResult? myApprenticeship, Apprenticeship? apprenticeShip)
    {
        FullName = memberProfileWithPreferences.FullName;
        Email = memberProfileWithPreferences.Email;
        FirstName = memberProfileWithPreferences.FirstName;
        LastName = memberProfileWithPreferences.LastName;
        OrganisationName = memberProfileWithPreferences.OrganisationName;
        RegionId = memberProfileWithPreferences.RegionId;
        JoinedDate = memberProfileWithPreferences.JoinedDate;
        RegionName = memberProfileWithPreferences.RegionName;
        UserType = memberProfileWithPreferences.UserType;
        IsRegionalChair = memberProfileWithPreferences.IsRegionalChair;
        Profiles = memberProfileWithPreferences.Profiles;
        Preferences = memberProfileWithPreferences.Preferences;

        if (memberProfileWithPreferences.UserType == MemberUserType.Apprentice && myApprenticeship != null && myApprenticeship.TrainingCourse != null)
        {
            Apprenticeship = new Apprenticeship
            {
                Level = myApprenticeship.TrainingCourse.Level.ToString(),
                Sector = myApprenticeship.TrainingCourse.Sector!,
                Programme = myApprenticeship.TrainingCourse.Name!
            };
        }
        else if (memberProfileWithPreferences.UserType == MemberUserType.Employer && apprenticeShip != null)
        {
            Apprenticeship = new Apprenticeship
            {
                ActiveApprenticesCount = apprenticeShip.ActiveApprenticesCount,
                Sectors = apprenticeShip.Sectors
            };
        }
    }
}
public class Apprenticeship
{
    public string Sector { get; set; } = null!;
    public string Programme { get; set; } = null!;
    public string Level { get; set; } = null!;
    public int ActiveApprenticesCount { get; set; }
    public IEnumerable<string> Sectors { get; set; } = Enumerable.Empty<string>();
}
