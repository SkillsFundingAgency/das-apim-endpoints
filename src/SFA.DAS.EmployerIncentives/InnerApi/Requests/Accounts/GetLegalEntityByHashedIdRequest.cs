using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class GetLegalEntityByHashedIdRequest : IGetApiRequest
    {
        public GetLegalEntityByHashedIdRequest(string hashedLegalEntityId)
        {
            HashedLegalEntityId = hashedLegalEntityId;
        }

        public string HashedLegalEntityId { get; private set; }
        public string GetUrl => "/legalentities/{hashedLegalEntityId}";
    }
}
