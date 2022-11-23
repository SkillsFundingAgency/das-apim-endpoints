using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetHealthCheckRequest : IGetApiRequest
    {
        public string GetUrl => "api/healthcheck";
    }
}