namespace SFA.DAS.Aodp.Application.Queries.Jobs
{
    public class GetJobRunsByNameQueryResponse
    {
        public List<JobRun> JobRuns { get; set; }
    }

    public class JobRun
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
