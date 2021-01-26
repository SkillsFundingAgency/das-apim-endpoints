using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.InnerApi.Requests
{
    public class GetStandardsRequest :IGetApiRequest
    {
        public string GetUrl => "api/courses/standards?filter=ActiveAvailable";
    }
}