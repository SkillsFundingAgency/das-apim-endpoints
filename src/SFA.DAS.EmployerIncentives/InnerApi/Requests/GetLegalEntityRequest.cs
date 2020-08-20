using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetLegalEntityRequest : IGetApiRequest
    {
        private readonly long _accountId;
        private readonly long _accountLegalEntityId;

        public GetLegalEntityRequest(long accountId, long accountLegalEntityId)
        {
            _accountId = accountId;
            _accountLegalEntityId = accountLegalEntityId;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}accounts/{_accountId}/legalentities/{_accountLegalEntityId}";
    }
}