using System;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Responses
{
    public class GetProviderResponse
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public int Ukprn { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string MarketingInfo { get; set; }
        public decimal? EmployerSatisfaction { get; set; }
        public decimal? LearnerSatisfaction { get; set; }
        public bool? HasConfirmedDetails { get; set; }
        public DateTime? ConfirmedDetailsOn { get; set; }
    }
}
