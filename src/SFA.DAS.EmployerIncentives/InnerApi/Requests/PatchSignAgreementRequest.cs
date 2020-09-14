using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PatchSignAgreementRequest : IPatchApiRequest<SignAgreementRequest>
    {
        private readonly long _accountId;
        private readonly long _accountLegalEntityId;

        public PatchSignAgreementRequest(long accountId, long accountLegalEntityId)
        {
            _accountId = accountId;
            _accountLegalEntityId = accountLegalEntityId;
        }

        public string PatchUrl => $"accounts/{_accountId}/legalentities/{_accountLegalEntityId}";
        public SignAgreementRequest Data { get; set; }
    }

    public class SignAgreementRequest
    {
        public int AgreementVersion { get; set; }
    }
}