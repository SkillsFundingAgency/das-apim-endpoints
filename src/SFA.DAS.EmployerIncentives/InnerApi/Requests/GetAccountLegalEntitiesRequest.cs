using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetAccountLegalEntitiesRequest : IGetApiRequest
    {
        private readonly long _accountId;

        public GetAccountLegalEntitiesRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}accounts/{_accountId}/legalentities";
    }
}