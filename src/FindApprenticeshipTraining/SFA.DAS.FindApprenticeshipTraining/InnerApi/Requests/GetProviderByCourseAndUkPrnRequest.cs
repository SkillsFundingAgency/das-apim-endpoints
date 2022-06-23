using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProviderByCourseAndUkPrnRequest : IGetApiRequest
    {
        private readonly int _providerId;
        private readonly int _courseId;
        private readonly string _sectorSubjectArea;
        private readonly double? _latitude;
        private readonly double? _longitude;
        private readonly Guid? _shortlistUserId;

        public GetProviderByCourseAndUkPrnRequest(int providerId, int courseId, string sectorSubjectArea, double? latitude = null, double? longitude = null, Guid? shortlistUserId = null)
        {
            _providerId = providerId;
            _courseId = courseId;
            _sectorSubjectArea = sectorSubjectArea;
            _latitude = latitude;
            _longitude = longitude;
            _shortlistUserId = shortlistUserId;
        }

        public string GetUrl => $"api/courses/{_courseId}/providers/{_providerId}?lat={_latitude}&lon={_longitude}&sectorSubjectArea={_sectorSubjectArea}&shortlistUserId={_shortlistUserId}";
    }
}