﻿namespace SFA.DAS.Funding.InnerApi.Responses
{
    public class OnProgrammeEarningDto
    {
        public short AcademicYear { get; set; }
        public byte DeliveryPeriod { get; set; }
        public decimal Amount { get; set; }
        public decimal? GovernmentContribution { get; set; }
        public decimal? EmployerContribution { get; set; }
    }
}