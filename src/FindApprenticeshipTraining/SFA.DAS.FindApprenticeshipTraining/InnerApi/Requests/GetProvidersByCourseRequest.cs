using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    //MFCMFC to be removed
    public class GetProvidersByCourseRequest : IGetApiRequest
    {
        private readonly double? _latitude;
        private readonly double? _longitude;
        private readonly int _sortOrder;
        private readonly Guid? _shortlistUserId;
        private readonly int _courseId;
        private readonly string _sectorSubjectArea;
        private readonly int _level;

        private const short AllLevels = 1;
        public GetProvidersByCourseRequest (int id, string sectorSubjectArea, int level,
            double? latitude = null, double? longitude = null, int sortOrder = 0, Guid? shortlistUserId = null)
        {
            _latitude = latitude;
            _longitude = longitude;
            _sortOrder = sortOrder;
            _shortlistUserId = shortlistUserId;
            _courseId = id;
            _sectorSubjectArea = sectorSubjectArea;
            _level = level;
            if (level > 3)
            {
                _level = AllLevels;
            }
        }
        
        public string GetUrl => $"api/courses/{_courseId}/providers?lat={_latitude}&lon={_longitude}&sortOrder={_sortOrder}&sectorSubjectArea={_sectorSubjectArea}&level={_level}&shortlistUserId={_shortlistUserId}";
    }
}