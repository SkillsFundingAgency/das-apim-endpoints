using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProvidersByCourseRequest : IGetApiRequest
    {
        private readonly double? _latitude;
        private readonly double? _longitude;
        private int CourseId { get ;}
        
        public GetProvidersByCourseRequest (int id, double? latitude = null, double? longitude = null)
        {
            _latitude = latitude;
            _longitude = longitude;
            CourseId = id;
        }
        public string BaseUrl { get; set; }
        
        public string GetUrl => $"{BaseUrl}api/courses/{CourseId}/providers?latitude={_latitude}&longitude={_longitude}";
    }
}