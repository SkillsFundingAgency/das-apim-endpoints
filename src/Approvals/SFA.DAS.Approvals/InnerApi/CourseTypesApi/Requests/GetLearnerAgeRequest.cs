using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests
{
    public class GetLearnerAgeRequest(string courseTypeShortCode) : IGetApiRequest
    {
        public string GetUrl => $"api/coursetypes/{courseTypeShortCode}/features/learnerAge";
    }
} 