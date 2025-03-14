namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetJobByNameQueryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public bool Enabled { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? LastRunTime { get; set; }
    }
}
