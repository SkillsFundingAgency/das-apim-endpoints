using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAccountLegalEntityRequest : IGetApiRequest
    {
        public long AccountLegalEntityId { get; set; }
        public string GetUrl => $"api/AccountLegalEntity/{AccountLegalEntityId}";

        public GetAccountLegalEntityRequest(long accountLegalEntityId)
        {
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}
