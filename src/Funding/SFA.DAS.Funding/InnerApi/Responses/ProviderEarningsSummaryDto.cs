﻿namespace SFA.DAS.Funding.InnerApi.Responses
{
    public class ProviderEarningsSummaryDto
    {
        public decimal TotalEarningsForCurrentAcademicYear { get; set; }
        public decimal TotalLevyEarningsForCurrentAcademicYear { get; set; }
        public decimal TotalNonLevyEarningsForCurrentAcademicYear { get; set; }
        public decimal TotalNonLevyEarningsForCurrentAcademicYearGovernment { get; set; }
        public decimal TotalNonLevyEarningsForCurrentAcademicYearEmployer { get; set; }
    }
}
