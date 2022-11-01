using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetAccountTeamMembersRequest : IGetAllApiRequest
    {
        private readonly string _accountId;

        public GetAccountTeamMembersRequest (string accountId)
        {
            _accountId = accountId;
        }
            
        public string GetAllUrl => $"api/accounts/{_accountId}/users";
    }
}