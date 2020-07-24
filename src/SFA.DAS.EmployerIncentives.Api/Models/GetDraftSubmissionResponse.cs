namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class GetDraftSubmissionResponse
    {
        public long AccountLegalEntityId { get; set; }
        public DraftSubmissionApprenticeshipDto[] Apprentices { get; set; }
    }
}
