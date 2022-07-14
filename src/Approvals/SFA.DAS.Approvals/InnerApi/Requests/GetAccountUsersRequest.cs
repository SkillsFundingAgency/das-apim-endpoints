using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAccountUsersRequest : IGetApiRequest
    {
        public string HashedAccountId { get; }

        public GetAccountUsersRequest(string hashedAccountId)
        {
            HashedAccountId = hashedAccountId;
        }

        public string GetUrl => $"api/accounts/{HashedAccountId}/users";
    }
}