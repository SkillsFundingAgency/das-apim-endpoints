using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetPingRequest : IGetApiRequest
    {
        public string GetUrl => "ping";
    }
}