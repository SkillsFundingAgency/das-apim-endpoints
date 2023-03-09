using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProviderByCourseAndUkprnRequest : IGetApiRequest
    {
        public int ProviderId { get; set; }
        public int CourseId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public GetProviderByCourseAndUkprnRequest(int providerId, int courseId, double? latitude = null, double? longitude = null)
        {
            ProviderId = providerId;
            CourseId = courseId;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string GetUrl => $"api/courses/{CourseId}/providers/{ProviderId}?lat={Latitude}&lon={Longitude}";
    }
}