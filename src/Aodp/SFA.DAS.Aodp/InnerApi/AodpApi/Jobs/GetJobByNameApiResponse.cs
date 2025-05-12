namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class GetJobByNameApiResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public bool Enabled { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? LastRunTime { get; set; }
    }   
}
