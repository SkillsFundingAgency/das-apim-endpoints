using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public class GetAccountLegalEntitiesRequest : IGetAllApiRequest
    {
        private readonly long _accountId;

        public GetAccountLegalEntitiesRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetAllUrl => $"api/accounts/{_accountId}/legalentities?includeDetails=true";
    }
}