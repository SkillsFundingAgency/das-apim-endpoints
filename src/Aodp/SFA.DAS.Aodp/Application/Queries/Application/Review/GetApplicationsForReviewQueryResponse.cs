using static SFA.DAS.Aodp.Application.Queries.Application.Review.GetApplicationForReviewByIdQueryResponse;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationsForReviewQueryResponse
    {
        public List<Application> Applications { get; set; } = new();
        public List<Reviewer> AvailableReviewers { get; set; } = new();
        public int TotalRecordsCount { get; set; }
        public class Application
        {
            public Guid Id { get; set; }
            public Guid ApplicationReviewId { get; set; }
            public string Name { get; set; }
            public DateTime LastUpdated { get; set; }
            public int Reference { get; set; }
            public string? Qan { get; set; }
            public string? AwardingOrganisation { get; set; }
            public string? Owner { get; set; }
            public string Status { get; set; }
            public bool NewMessage { get; set; }
            public string Reviewer1 { get; set; }
            public string Reviewer2 { get; set; }

        }
    }
}

