namespace SFA.DAS.TrackProgressInternal.Apis.CoursesInnerApi;

public class GetKsbsForCourseOptionResponse
{
    public List<CourseKsb> Ksbs { get; set; } = new List<CourseKsb>(); 
}

public class CourseKsb
{
    public string Type { get; set; } = null!;
    public Guid Id { get; set; }
}