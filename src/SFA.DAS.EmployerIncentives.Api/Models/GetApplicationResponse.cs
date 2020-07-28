namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class GetApplicationResponse
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public ApplicationApprenticeshipDto[] Apprentices { get; set; }
    }
}
