using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetAllStandardsRequest : IGetApiRequest
    {
        public string GetUrl => BuildUrl();
        public CourseType? CourseType { get; }
        private string BuildUrl()
        {
            var url = $"standards";

            if (CourseType.HasValue)
            {
                url += $"?coursetype={CourseType}";
            }

            return url;
        }

        public GetAllStandardsRequest(CourseType? courseType)
        {
            CourseType = courseType;
        }
    }
}
