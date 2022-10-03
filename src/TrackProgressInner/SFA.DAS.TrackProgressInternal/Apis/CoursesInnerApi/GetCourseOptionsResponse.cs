namespace SFA.DAS.TrackProgressInternal.Apis.CoursesInnerApi
{
    public class GetCourseOptionsResponse
    {
        public string StandardUId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>(); 
    }
}
