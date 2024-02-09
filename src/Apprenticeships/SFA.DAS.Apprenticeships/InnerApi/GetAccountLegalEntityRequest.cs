using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Apprenticeships.InnerApi
{
    public class GetAccountLegalEntityRequest : IGetApiRequest
    {
        public readonly long AccountLegalEntityId;
        public string GetUrl => $"api/AccountLegalEntity/{AccountLegalEntityId}";

        public GetAccountLegalEntityRequest(long accountLegalEntityId)
        {
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}
