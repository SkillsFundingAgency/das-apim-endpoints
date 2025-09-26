namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public record ApplicationReviewStats
    {
        public long VacancyReference { get; set; } = 0;
        public int Applications { get; set; } = 0;
        public int NewApplications { get; set; } = 0;
        public int SharedApplications { get; set; } = 0;
        public int AllSharedApplications { get; set; } = 0;
        public int SuccessfulApplications { get; set; } = 0;
        public int UnsuccessfulApplications { get; set; } = 0;
        public int EmployerReviewedApplications { get; set; } = 0;
        public bool HasNoApplications { get; set; } = false;
    }
}