using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

[ExcludeFromCodeCoverage]
public class GetShortlistsForUserResponse
{
    public Guid UserId { get; set; }
    public DateTime MaxCreatedDate { get; set; }
    public DateTime ShortlistsExpiryDate { get; set; }
    public string QarPeriod { get; set; }
    public string ReviewPeriod { get; set; }
    public List<ShortlistCourseModel> Courses { get; set; } = [];
}

public class ShortlistCourseModel
{
    public int Ordering { get; set; }
    public string LarsCode { get; set; }
    public string StandardName { get; set; }
    public List<ShortlistLocationModel> Locations { get; set; } = [];
}

public class ShortlistLocationModel
{
    public int Ordering { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string LocationDescription { get; set; }
    public List<ShortlistProviderModel> Providers { get; set; } = [];
}

public class ShortlistProviderModel
{
    public int Ordering { get; set; }
    public Guid ShortlistId { get; set; }
    public int Ukprn { get; set; }
    public string ProviderName { get; set; }
    public bool AtEmployer { get; set; }
    public bool HasBlockRelease { get; set; }
    public decimal? BlockReleaseDistance { get; set; }
    public int BlockReleaseCount { get; set; }
    public bool HasDayRelease { get; set; }
    public decimal? DayReleaseDistance { get; set; }
    public int DayReleaseCount { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
    public string Leavers { get; set; }
    public string QarPeriod { get; set; }
    public string ReviewPeriod { get; set; }
    public string AchievementRate { get; set; }
    public string EmployerReviews { get; set; }
    public string EmployerStars { get; set; }
    public string EmployerRating { get; set; }
    public string ApprenticeReviews { get; set; }
    public string ApprenticeStars { get; set; }
    public string ApprenticeRating { get; set; }
}
