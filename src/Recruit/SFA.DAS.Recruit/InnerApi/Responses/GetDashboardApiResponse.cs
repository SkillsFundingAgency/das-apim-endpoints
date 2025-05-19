namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public record GetDashboardApiResponse
    {
        public int NewApplicationsCount { get; set; } = 0;
        public int EmployerReviewedApplicationsCount { get; set; } = 0;
    }
}