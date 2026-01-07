using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetAllStandardsRequest : IGetApiRequest
    {
        public string GetUrl => $"standards?coursetype={CourseType}";
        public CourseType? CourseType { get; }

        public GetAllStandardsRequest(CourseType? courseType)
        {
            CourseType = courseType;
        }
    }
}
