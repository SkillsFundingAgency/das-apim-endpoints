using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetAvailableToStartStandardsListRequest : IGetApiRequest
    {
        public string Keyword { get ; set ; }
        public CoursesOrderBy OrderBy { get; set; }
        public List<int> RouteIds { get; set; }
        public List<int> Levels { get ; set ; }
        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var url = $"api/courses/standards?keyword={Keyword}&orderby={OrderBy}&filter=ActiveAvailable";

            if (RouteIds != null && RouteIds.Any())
            {
                url += "&routeIds=" + string.Join("&routeIds=", RouteIds);
            }

            if (Levels != null && Levels.Any())
            {
                url += "&levels=" + string.Join("&levels=", Levels);
            }

            return url;
        }
    }

    public enum CoursesOrderBy
    {
        Score,
        Title
    }
}
