namespace SFA.DAS.FindApprenticeshipJobs.Domain.Models
{
    public record PatchSavedSearch
    {
        public DateTime? LastRunDate { get; set; }
        public DateTime? EmailLastSendDate { get; set; }
    }
}