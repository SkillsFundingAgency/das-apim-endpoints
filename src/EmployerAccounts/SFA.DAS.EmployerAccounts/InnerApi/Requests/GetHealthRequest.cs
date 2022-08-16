using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.InnerApi.Requests
{
    public class GetHealthRequest : IGetApiRequest
    {
        public string GetUrl => "ping";
    }
}
