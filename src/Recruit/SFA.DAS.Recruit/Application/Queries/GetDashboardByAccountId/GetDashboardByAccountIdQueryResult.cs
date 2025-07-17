namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId
{
    public record GetDashboardByAccountIdQueryResult
    {
        public int NewApplicationsCount { get; set; } = 0;
        public int EmployerReviewedApplicationsCount { get; set; } = 0;
        public int SharedApplicationsCount { get; set; } = 0;
        public int AllSharedApplicationsCount { get; set; } = 0;
        public int SuccessfulApplicationsCount { get; set; } = 0;
        public int UnsuccessfulApplicationsCount { get; set; } = 0;
        public bool HasNoApplications { get; set; } = false;

        public static implicit operator GetDashboardByAccountIdQueryResult(InnerApi.Responses.GetDashboardApiResponse response)
        {
            return new GetDashboardByAccountIdQueryResult
            {
                NewApplicationsCount = response.NewApplicationsCount,
                EmployerReviewedApplicationsCount = response.EmployerReviewedApplicationsCount,
                SharedApplicationsCount = response.SharedApplicationsCount,
                AllSharedApplicationsCount = response.AllSharedApplicationsCount,
                SuccessfulApplicationsCount = response.SuccessfulApplicationsCount,
                UnsuccessfulApplicationsCount = response.UnsuccessfulApplicationsCount,
                HasNoApplications = response.HasNoApplications
            };
        }
    }
}