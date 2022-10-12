using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgress.Apis.CoursesInnerApi
{
    public class GetCourseOptionsRequest : IGetApiRequest
    {
        public string _standardUId { get; set; } = String.Empty;

        public GetCourseOptionsRequest(string standardUId)
                => _standardUId = standardUId;

        public string GetUrl => $"api/courses/standards/{_standardUId}";
    }
}
