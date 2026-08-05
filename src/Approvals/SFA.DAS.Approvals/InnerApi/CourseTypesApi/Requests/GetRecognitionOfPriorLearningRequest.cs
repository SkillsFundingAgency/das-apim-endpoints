using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests
{
    public class GetRecognitionOfPriorLearningRequest(string courseTypeShortCode) : IGetApiRequest
    {
        public string GetUrl => $"api/coursetypes/{courseTypeShortCode}/features/rpl";
    }
} 