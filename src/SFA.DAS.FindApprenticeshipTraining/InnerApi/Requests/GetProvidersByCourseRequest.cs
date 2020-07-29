using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests
{
    public class GetProvidersByCourseRequest : IGetApiRequest
    {
        private int _courseId { get ; set ; }
        
        public GetProvidersByCourseRequest (int id)
        {
            _courseId = id;
        }
        public string BaseUrl { get; set; }
        
        public string GetUrl => $"{BaseUrl}api/courses/{_courseId}/providers";
    }
}