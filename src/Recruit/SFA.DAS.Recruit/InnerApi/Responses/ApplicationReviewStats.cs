namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public record ApplicationReviewStats
    {
        public long VacancyReference { get; set; }
        public int NewApplications { get; set; }
        public int SuccessfulApplications { get; set; }
        public int UnsuccessfulApplications { get; set; }
        public int Applications { get; set; }
    }
}