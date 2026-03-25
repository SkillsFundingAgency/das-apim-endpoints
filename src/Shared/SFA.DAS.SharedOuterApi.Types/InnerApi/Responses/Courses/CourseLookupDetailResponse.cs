using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses
{
    [ExcludeFromCodeCoverage]
    public class CourseLookupDetailResponse 
    {
        public string LarsCode { get; set; }
        public string Title { get; set; }
        public string LearningType { get; set; }
        public string CourseType { get; set; }
    }
}
