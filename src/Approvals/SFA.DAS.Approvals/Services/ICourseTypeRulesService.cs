using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Services
{
    public interface ICourseTypeRulesService
    {
        Task<CourseTypeRulesResult> GetCourseTypeRulesAsync(string courseCode);
        Task<RplRulesResult> GetRplRulesAsync(string courseCode);
    }

    public class CourseTypeRulesResult
    {
        public GetStandardsListItem Standard { get; set; }
        public GetLearnerAgeResponse LearnerAgeRules { get; set; }
    }

    public class RplRulesResult
    {
        public GetStandardsListItem Standard { get; set; }
        public GetRecognitionOfPriorLearningResponse RplRules { get; set; }
    }
} 