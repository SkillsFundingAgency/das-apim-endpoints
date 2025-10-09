using SFA.DAS.Recruit.InnerApi.Recruit.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn
{
    public record GetDashboardByUkprnQueryResult
    {
        public int NewApplicationsCount { get; set; } = 0;
        public int EmployerReviewedApplicationsCount { get; set; } = 0;
        public int SharedApplicationsCount { get; set; } = 0;
        public int AllSharedApplicationsCount { get; set; } = 0;
        public int SuccessfulApplicationsCount { get; set; } = 0;
        public int UnsuccessfulApplicationsCount { get; set; } = 0;
        public bool HasNoApplications { get; set; } = false;

        public static implicit operator GetDashboardByUkprnQueryResult(GetDashboardApiResponse response)
        {
            return new GetDashboardByUkprnQueryResult
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