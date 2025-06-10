namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn
{
    public record GetDashboardByUkprnQueryResult
    {
        public int NewApplicationsCount { get; set; } = 0;
        public int EmployerReviewedApplicationsCount { get; set; } = 0;

        public static implicit operator GetDashboardByUkprnQueryResult(InnerApi.Responses.GetDashboardApiResponse response)
        {
            return new GetDashboardByUkprnQueryResult
            {
                NewApplicationsCount = response.NewApplicationsCount,
                EmployerReviewedApplicationsCount = response.EmployerReviewedApplicationsCount
            };
        }
    }
}