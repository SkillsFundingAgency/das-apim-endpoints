//using System.Threading.Tasks;
//using SFA.DAS.Approvals.InnerApi.CoursesApi;
//using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;

//namespace SFA.DAS.Approvals.Services
//{
//    public interface ICourseTypeRulesServiceWithCourses
//    {
//        Task<CourseTypeRulesResultWithCourses> GetCourseTypeRulesAsync(string courseCode);
//        Task<RplRulesResultWithCourses> GetRplRulesAsync(string courseCode);
//    }

//    public class CourseTypeRulesResultWithCourses
//    {
//        public GetCourseLookupResponse Course { get; set; }
//        public GetLearnerAgeResponse LearnerAgeRules { get; set; }
//    }

//    public class RplRulesResultWithCourses
//    {
//        public GetCourseLookupResponse Course { get; set; }
//        public GetRecognitionOfPriorLearningResponse RplRules { get; set; }
//    }
//} 