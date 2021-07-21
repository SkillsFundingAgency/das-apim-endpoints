using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class CreateApplicationRequest : IPostApiRequest
    {
        public int PledgeId { get; private set; }

        public CreateApplicationRequest(int pledgeId, long accountId)
        {
            PledgeId = pledgeId;
            Data = new CreateApplicationRequestData
            {
                EmployerAccountId = accountId
            };
        }
        
        public class CreateApplicationRequestData
        {
            public long EmployerAccountId { get; set; }
        }

        public string PostUrl => $"/pledges/{PledgeId}/applications";
        public object Data { get; set; }
    }
}
