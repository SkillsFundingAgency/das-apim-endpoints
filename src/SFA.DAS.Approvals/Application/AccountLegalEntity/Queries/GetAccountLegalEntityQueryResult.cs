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


        public static implicit operator GetAccountLegalEntityQueryResult(InnerApi.Responses.GetAccountLegalEntityResponse result)
        {
            return new GetAccountLegalEntityQueryResult
            {
                AccountId = result.AccountId,
                MaLegalEntityId = result.MaLegalEntityId,
                AccountName = result.AccountName,
                LegalEntityName = result.LegalEntityName,
                LevyStatus = (Shared.Enums.ApprenticeshipEmployerType)result.LevyStatus
            };
        }
    }
}
