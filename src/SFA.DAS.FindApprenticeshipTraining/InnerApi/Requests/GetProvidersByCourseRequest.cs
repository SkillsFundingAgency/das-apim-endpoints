using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetProvidersByCourseRequest : IGetApiRequest
    {
        private readonly double? _latitude;
        private readonly double? _longitude;
        private readonly int _sortOrder;
        private readonly int _courseId;
        private readonly string _sectorSubjectArea;
        private readonly int _level;

        public GetProvidersByCourseRequest (  int id,  string sectorSubjectArea, int level, double? latitude = null, double? longitude = null, int sortOrder = 0)
        {
            _latitude = latitude;
            _longitude = longitude;
            _sortOrder = sortOrder;
            _courseId = id;
            _sectorSubjectArea = sectorSubjectArea;
            _level = level;
        }
        
        public string GetUrl => $"api/courses/{_courseId}/providers?lat={_latitude}&lon={_longitude}&sortOrder={_sortOrder}&sectorSubjectArea={_sectorSubjectArea}&level={_level}";
    }
}