using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAccountRequest : IGetApiRequest
    {
        public string HashedAccountId { get; }

        public GetAccountRequest(string hashedAccountId)
        {
            HashedAccountId = hashedAccountId;
        }

        public string GetUrl => $"api/accounts/{HashedAccountId}";
    }
}