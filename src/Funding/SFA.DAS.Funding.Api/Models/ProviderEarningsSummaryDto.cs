using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Api.Models
{
    public class ProviderEarningsSummaryDto
    {
        public decimal TotalEarningsForCurrentAcademicYear { get; set; }

        public static implicit operator ProviderEarningsSummaryDto(ProviderEarningsSummary source)
        {
            return new ProviderEarningsSummaryDto
            {
                TotalEarningsForCurrentAcademicYear = source.TotalEarningsForCurrentAcademicYear
            };
        }
    }
}
