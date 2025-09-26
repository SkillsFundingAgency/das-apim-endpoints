using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;

public class ProviderLocation
{
    public int Ordering { get; set; }
    public LocationType LocationType { get; set; }
    public bool AtEmployer { get; set; }
    public bool DayRelease { get; set; }
    public bool BlockRelease { get; set; }

    public decimal CourseDistance { get; set; }
}