using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses
{
    public class ProviderResponse
    {
        public int Ukprn { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public DateTime RespondedAt { get; set; }
    }
}
