using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments
{
    public class GetEmployerCohortsReadyForApprovalResponse
    {
        public IEnumerable<EmployerCohortsReadyForApprovalResponse> EmployerCohortsReadyForApprovalResponse { get; set; }
    }

    public class EmployerCohortsReadyForApprovalResponse
    {
        public long CohortId { get; set; }
        public string CohortReference { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public long? ProviderId { get; set; }
        public string ProviderName { get; set; }
        public long? TransferSenderId { get; set; }
        public string TransferSenderName { get; set; }
    }
}