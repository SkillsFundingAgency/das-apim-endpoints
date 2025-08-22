namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses
{
    public record GetDashboardApiResponse
    {
        public int NewApplicationsCount { get; set; } = 0;
        public int EmployerReviewedApplicationsCount { get; set; } = 0;
        public int SharedApplicationsCount { get; set; } = 0;
        public int AllSharedApplicationsCount { get; set; } = 0;
        public int SuccessfulApplicationsCount { get; set; } = 0;
        public int UnsuccessfulApplicationsCount { get; set; } = 0;
        public bool HasNoApplications { get; set; } = false;
    }
}