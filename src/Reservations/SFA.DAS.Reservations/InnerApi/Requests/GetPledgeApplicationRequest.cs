using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetPledgeApplicationRequest(int applicationId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{applicationId}";
    }
}
