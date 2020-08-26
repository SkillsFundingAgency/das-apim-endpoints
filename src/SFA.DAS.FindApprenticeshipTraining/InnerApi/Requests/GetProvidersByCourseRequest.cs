using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProvidersByCourseRequest : IGetApiRequest
    {
        private readonly double? _latitude;
        private readonly double? _longitude;
        private readonly int _courseId;
        
        public GetProvidersByCourseRequest (int id, double? latitude = null, double? longitude = null)
        {
            _latitude = latitude;
            _longitude = longitude;
            _courseId = id;
        }
        public string BaseUrl { get; set; }
        
        public string GetUrl => $"{BaseUrl}api/courses/{_courseId}/providers?latitude={_latitude}&longitude={_longitude}";
    }
}