using System;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;

public class ProviderData
{
    public long Ordering { get; set; }
    public int Ukprn { get; set; }
    public string ProviderName { get; set; }
    public Guid? ShortlistId { get; set; }
    public List<ProviderLocation> Locations { get; set; }
    public string Leavers { get; set; }
    public string AchievementRate { get; set; }
    public string EmployerReviews { get; set; }
    public string EmployerStars { get; set; }
    public ProviderRating EmployerRating { get; set; }
    public string ApprenticeReviews { get; set; }
    public string ApprenticeStars { get; set; }
    public ProviderRating ApprenticeRating { get; set; }
}