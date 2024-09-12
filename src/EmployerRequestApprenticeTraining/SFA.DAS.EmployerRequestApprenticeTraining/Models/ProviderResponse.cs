using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class ProviderResponse
    {
        public int Ukprn { get; set; }
        public string ProviderName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public DateTime RespondedAt { get; set; }

        public static explicit operator ProviderResponse(InnerApi.Responses.ProviderResponse source)
        {
            if (source == null) return null;

            return new ProviderResponse()
            {
                Ukprn = source.Ukprn,
                ContactName = source.ContactName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                Website = source.Website,
                RespondedAt = source.RespondedAt
            };
        }
    }
}
