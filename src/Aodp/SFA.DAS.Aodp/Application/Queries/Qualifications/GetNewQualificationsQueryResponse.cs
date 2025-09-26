namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetNewQualificationsQueryResponse
    {
        public int TotalRecords { get; set; }
        public int? Skip { get; set; }
        public int Take { get; set; }        
        public List<NewQualification> Data { get; set; } = new();
        public Job Job { get; set; } = new();
    }
    public class NewQualification
    {
        public string? Title { get; set; }
        public string? Reference { get; set; }
        public string? AwardingOrganisation { get; set; }
        public string? Status { get; set; }
        public string? AgeGroup { get; set; }
    }

    public class Job
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? LastRunTime { get; set; }
    }
}
