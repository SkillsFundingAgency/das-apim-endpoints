using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProviderByCourseAndUkprnRequest : IGetApiRequest
    {
        public int ProviderId { get; set; }
        public int CourseId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public GetProviderByCourseAndUkprnRequest(int providerId, int courseId, decimal? latitude = null, decimal? longitude = null)
        {
            ProviderId = providerId;
            CourseId = courseId;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string GetUrl => $"api/courses/{CourseId}/providers/{ProviderId}?latitude={Latitude}&longitude={Longitude}";
    }
}
