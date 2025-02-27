namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetNewQualificationsQueryResponse
    {
        public List<NewQualification> NewQualifications { get; set; } = new();
    }
    public class NewQualification
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Reference { get; set; }
        public string? AwardingOrganisation { get; set; }
        public string? Status { get; set; }
    }
}
