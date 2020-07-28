namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class GetDraftSubmissionResponse
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public DraftSubmissionApprenticeshipDto[] Apprentices { get; set; }
    }
}
