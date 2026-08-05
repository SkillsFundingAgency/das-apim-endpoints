using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses
{
    public class GetStandardsLookupResponseFromCoursesApi
    {
        public List<GetStandardResponseFromCoursesApi> Courses { get; set; }
    }

    public class GetStandardsLookupResponse
    {
        public List<GetStandardResponse> Standards { get; set; }
    }
}
