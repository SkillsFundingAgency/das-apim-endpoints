using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts
{
    public class GetAccountByIdRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetAccountByIdRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/{AccountId}";
    }
}