using SFA.DAS.Approvals.Application.Shared.Enums;

namespace SFA.DAS.Approvals.Application.AccountLegalEntity
{
    public class GetAccountLegalEntityQueryResult
    {
        public long AccountId { get; set; }
        public long MaLegalEntityId { get; set; }
        public string AccountName { get; set; }
        public string LegalEntityName { get; set; }
        public ApprenticeshipEmployerType LevyStatus { get; set; }
    }
}
