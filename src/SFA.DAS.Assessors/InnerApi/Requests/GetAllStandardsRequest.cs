using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Assessors.InnerApi.Requests
{
    public class GetAllStandardsRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/standards";
    }
}
