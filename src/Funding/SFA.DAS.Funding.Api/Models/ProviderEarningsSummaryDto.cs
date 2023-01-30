using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Api.Models
{
    public class ProviderEarningsSummaryDto
    {
        public decimal TotalEarningsForCurrentAcademicYear { get; set; }
        public decimal TotalLevyEarningsForCurrentAcademicYear { get; set; }
        public decimal TotalNonLevyEarningsForCurrentAcademicYear { get; set; }
        public decimal TotalNonLevyEarningsForCurrentAcademicYearGovernment { get; set; }
        public decimal TotalNonLevyEarningsForCurrentAcademicYearEmployer { get; set; }

        public static implicit operator ProviderEarningsSummaryDto(ProviderEarningsSummary source)
        {
            return new ProviderEarningsSummaryDto
            {
                TotalEarningsForCurrentAcademicYear = source.TotalEarningsForCurrentAcademicYear,
                TotalLevyEarningsForCurrentAcademicYear = source.TotalLevyEarningsForCurrentAcademicYear,
                TotalNonLevyEarningsForCurrentAcademicYear = source.TotalNonLevyEarningsForCurrentAcademicYear,
                TotalNonLevyEarningsForCurrentAcademicYearGovernment = source.TotalNonLevyEarningsForCurrentAcademicYearGovernment,
                TotalNonLevyEarningsForCurrentAcademicYearEmployer = source.TotalNonLevyEarningsForCurrentAcademicYearEmployer
            };
        }
    }
}
