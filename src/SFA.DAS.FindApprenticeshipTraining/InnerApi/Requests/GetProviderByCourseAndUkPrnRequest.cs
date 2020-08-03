using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProviderByCourseAndUkPrnRequest : IGetApiRequest
    {
        private readonly int _providerId;
        private readonly int _courseId;

        public GetProviderByCourseAndUkPrnRequest(int providerId, int courseId)
        {
            _providerId = providerId;
            _courseId = courseId;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/courses/{_courseId}/providers/{_providerId}";
    }
}