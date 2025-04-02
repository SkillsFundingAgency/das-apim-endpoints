namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId
{
    public record GetDashboardByAccountIdQueryResult
    {
        public int NewApplicationsCount { get; set; } = 0;
        public int EmployerReviewedApplicationsCount { get; set; } = 0;

        public static implicit operator GetDashboardByAccountIdQueryResult(InnerApi.Responses.GetDashboardApiResponse response)
        {
            return new GetDashboardByAccountIdQueryResult
            {
                NewApplicationsCount = response.NewApplicationsCount,
                EmployerReviewedApplicationsCount = response.EmployerReviewedApplicationsCount
            };
        }
    }
}