namespace SFA.DAS.Reservations.Api.Models
{
    public class GetProviderResponse
    {
        public long Id { get; set; }
        public uint Ukprn { get; set; }
        public string Name { get; set; }
        public bool NationalProvider { get; set; }
        public decimal? LearnerSatisfaction { get; set; }
        public decimal? EmployerSatisfaction { get; set; }
        public string TradingName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }

        public static implicit operator GetProviderResponse(
            InnerApi.Responses.GetProviderResponse source)
        {
            if (source == null)
            {
                return null;
            }
            return new GetProviderResponse
            {
                Id = source.Id,
                Ukprn = source.Ukprn,
                Name = source.Name,
                NationalProvider = source.NationalProvider,
                LearnerSatisfaction = source.LearnerSatisfaction,
                EmployerSatisfaction = source.EmployerSatisfaction,
                TradingName = source.TradingName,
                Email = source.Email,
                Website = source.Website,
                Phone = source.Phone
            };
        }
    }
}