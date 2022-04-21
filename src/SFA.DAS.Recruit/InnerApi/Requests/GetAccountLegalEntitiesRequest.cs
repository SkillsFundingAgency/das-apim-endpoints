using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public class GetAccountLegalEntitiesRequest : IGetAllApiRequest
    {
        private readonly string _hashedAccountId;

        public GetAccountLegalEntitiesRequest(string hashedAccountId)
        {
            _hashedAccountId = hashedAccountId;
        }

        public string GetAllUrl => $"api/accounts/{_hashedAccountId}/legalentities?includeDetails=true";
    }
}