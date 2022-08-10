using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetAllStandardsRequest : IGetApiRequest
    {
        public string GetUrl => "standards";
    }
}
