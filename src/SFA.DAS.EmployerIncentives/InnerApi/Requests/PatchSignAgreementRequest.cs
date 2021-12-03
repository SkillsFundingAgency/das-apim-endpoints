using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PatchSignAgreementRequest : IPatchApiRequest<SignAgreementRequest>
    {
        public string PatchUrl => $"accounts/{Data.AccountId}/legalentities/{Data.AccountLegalEntityId}";
        public SignAgreementRequest Data { get; set; }
    }

    public class SignAgreementRequest
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public long LegalEntityId { get; set; }
        public int AgreementVersion { get; set; }
    }
}