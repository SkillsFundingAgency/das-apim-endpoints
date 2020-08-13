using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetAccountLegalEntitiesRequest : IGetAllApiRequest
    {
        private readonly long _accountId;

        public GetAccountLegalEntitiesRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string BaseUrl { get; set; }
        public string GetAllUrl => $"{BaseUrl}accounts/{_accountId}/legalentities";
    }
}