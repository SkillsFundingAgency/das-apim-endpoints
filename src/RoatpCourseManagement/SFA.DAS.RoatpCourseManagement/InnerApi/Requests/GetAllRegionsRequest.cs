using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetAllRegionsRequest : IGetApiRequest
    {
        public string GetUrl => $"lookup/regions";
    }
}
