using System;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class StandardVersionDetail
    {
        public DateTime? EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public int TypicalDuration { get; set; }
        public int MaxFunding { get; set; }
    }
}