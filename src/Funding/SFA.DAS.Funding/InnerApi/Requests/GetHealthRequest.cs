using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.InnerApi.Requests
{
    public class GetHealthRequest : IGetApiRequest
    {
        public string GetUrl => "ping";
    }
}