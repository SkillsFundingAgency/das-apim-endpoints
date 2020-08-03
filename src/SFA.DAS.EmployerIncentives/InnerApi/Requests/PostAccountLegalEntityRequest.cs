using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostAccountLegalEntityRequest : IPostApiRequest
    {
        private readonly long _accountId;

        public PostAccountLegalEntityRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string BaseUrl { get; set; }
        public string Version { get; }
        public string PostUrl => $"{BaseUrl}accounts/{_accountId}/legalentities";
        public object Data { get; set; }
    }

    public class AccountLegalEntityCreateRequest
    {
        public long AccountLegalEntityId { get; set; }
        public long LegalEntityId { get; set; }
        public string OrganisationName { get; set; }
    }
}