using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses
{
    [ExcludeFromCodeCoverage]
    public class CourseLookupDetailResponse 
    {
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public string LearningType { get; set; }
        public string CourseType { get; set; }
    }
}
