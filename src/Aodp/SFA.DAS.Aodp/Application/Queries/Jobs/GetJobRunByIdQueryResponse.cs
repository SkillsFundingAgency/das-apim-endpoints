namespace SFA.DAS.Aodp.Application.Queries.Jobs
{
    public class GetJobRunByIdQueryResponse
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string User { get; set; } = null!;

        public int? RecordsProcessed { get; set; }

        public Guid JobId { get; set; }

        public static implicit operator GetJobRunByIdQueryResponse(JobRun jobRun)
        {
            GetJobRunByIdQueryResponse response = new()      
                {
                    Id = jobRun.Id,
                    Status = jobRun.Status,
                    StartTime = jobRun.StartTime,
                    EndTime = jobRun.EndTime,
                    User = jobRun.User,
                    RecordsProcessed = jobRun.RecordsProcessed,
                    JobId = jobRun.JobId,
                };
            
            return response;
        }
    }
}
