using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges
{
    public class GetPledgeRequest : IGetApiRequest
    {
        private readonly int _id;

        public GetPledgeRequest(int id)
        {
            _id = id;
        }

        public string GetUrl => $"pledges/{_id}";
    }
}