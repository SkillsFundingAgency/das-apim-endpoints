using System;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Responses
{
    public class Provider
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

        public static implicit operator Provider(GetProviderResponse provideResponse)
            => new Provider()
            {
                Id = provideResponse.Id,
                ExternalId = provideResponse.ExternalId,
                Ukprn = provideResponse.Ukprn,
                LegalName = provideResponse.LegalName,
                TradingName = provideResponse.TradingName,
                Email = provideResponse.Email,
                Phone = provideResponse.Phone,
                Website = provideResponse.Website,
                MarketingInfo = provideResponse.MarketingInfo,
                EmployerSatisfaction = provideResponse.EmployerSatisfaction,
                LearnerSatisfaction = provideResponse.LearnerSatisfaction,
                HasConfirmedDetails = provideResponse.HasConfirmedDetails,
                ConfirmedDetailsOn = provideResponse.ConfirmedDetailsOn,
            };
    }
}
