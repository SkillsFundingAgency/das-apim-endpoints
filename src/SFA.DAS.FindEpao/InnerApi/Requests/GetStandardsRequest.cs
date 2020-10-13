using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.InnerApi.Requests
{
    public class GetStandardsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/courses/standards";
    }
}