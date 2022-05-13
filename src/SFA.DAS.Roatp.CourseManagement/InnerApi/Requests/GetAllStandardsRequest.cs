using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    
    public class GetAllStandardsRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/Standards?Filter=Active";
    }
}
