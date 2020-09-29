using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetHealthRequest : IGetApiRequest
    {
        public string GetUrl => "ping";
    }
}