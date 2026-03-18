using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetPledgeApplicationRequest : IGetApiRequest
    {
        public int PledgeApplicationId { get; }

        public GetPledgeApplicationRequest(int pledgeApplicationId)
        {
            PledgeApplicationId = pledgeApplicationId;
        }
        public string GetUrl => $"applications/{PledgeApplicationId}";
    }
}
