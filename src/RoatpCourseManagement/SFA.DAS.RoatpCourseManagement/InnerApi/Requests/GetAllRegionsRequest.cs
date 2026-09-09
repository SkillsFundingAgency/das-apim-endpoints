using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetAllRegionsRequest : IGetApiRequest
    {
        public string GetUrl => $"lookup/regions";
    }
}
