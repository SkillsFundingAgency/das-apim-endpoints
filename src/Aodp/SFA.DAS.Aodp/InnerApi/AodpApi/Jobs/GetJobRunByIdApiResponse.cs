namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class GetJobRunByIdApiResponse
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string User { get; set; } = null!;

        public int? RecordsProcessed { get; set; }

        public Guid JobId { get; set; }
    }   
}
