using System;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Responses.Domain.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public int Ukprn { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string MarketingInfo { get; set; }
        public decimal? EmployerSatisfaction { get; set; }
        public decimal? LearnerSatisfaction { get; set; }
        public bool IsImported { get; set; } = false;
        public bool? HasConfirmedLocations { get; set; } //Required if imported
        public bool? HasConfirmedDetails { get; set; } //Required if imported

        public static implicit operator Provider(GetProviderResponse provideResponse)
            => provideResponse == null ? null : 
            new Provider()
            {
                Id = provideResponse.Id,
                Ukprn = provideResponse.Ukprn,
                LegalName = provideResponse.LegalName,
                TradingName = provideResponse.TradingName,
                Email = provideResponse.Email,
                Phone = provideResponse.Phone,
                Website = provideResponse.Website,
                MarketingInfo = provideResponse.MarketingInfo,
                EmployerSatisfaction = provideResponse.EmployerSatisfaction,
                LearnerSatisfaction = provideResponse.LearnerSatisfaction,
                IsImported = provideResponse.IsImported,
                HasConfirmedLocations = provideResponse.HasConfirmedLocations,
                HasConfirmedDetails = provideResponse.HasConfirmedDetails,
            };
    }
}
