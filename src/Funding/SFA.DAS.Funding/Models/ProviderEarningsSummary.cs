namespace SFA.DAS.Funding.Models
{
    public class ProviderEarningsSummary
    {
        public decimal TotalEarningsForCurrentAcademicYear { get; set; }
        public decimal TotalLevyEarningsForCurrentAcademicYear { get; set; }
        public decimal TotalNonLevyEarningsForCurrentAcademicYear { get; set; }
        public long Ukprn { get; set; }
    }
}
