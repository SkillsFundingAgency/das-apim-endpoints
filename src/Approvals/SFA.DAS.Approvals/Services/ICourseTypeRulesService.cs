using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;

namespace SFA.DAS.Approvals.Services
{
    public interface ICourseTypeRulesService
    {
        Task<CourseTypeRulesResult> GetCourseTypeRulesAsync(string courseCode);
        Task<RplRulesResult> GetRplRulesAsync(string courseCode);
    }

    public class CourseTypeRulesResult
    {
        public GetCourseLookupResponse Course { get; set; }
        public GetLearnerAgeResponse LearnerAgeRules { get; set; }
    }

    public class RplRulesResult
    {
        public GetCourseLookupResponse Course { get; set; }
        public GetRecognitionOfPriorLearningResponse RplRules { get; set; }
    }
}