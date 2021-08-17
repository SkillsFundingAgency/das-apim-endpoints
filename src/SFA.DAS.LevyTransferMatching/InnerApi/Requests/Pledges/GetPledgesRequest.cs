using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges
{
    public class GetPledgesRequest : IGetAllApiRequest
    {
        public string GetAllUrl => $"pledges";
    }
}
