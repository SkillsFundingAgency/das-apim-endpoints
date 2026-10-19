namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record GetPriceEpisodesResponse
{
    public IReadOnlyCollection<PriceEpisode> PriceEpisodes { get; set; } = [];

    public sealed record PriceEpisode
    {
        public long Id { get; set; }
        public long ApprenticeshipId { get; set; }
        public decimal Cost { get; set; }
        public decimal? TrainingPrice { get; set; }
        public decimal? EndPointAssessmentPrice { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}