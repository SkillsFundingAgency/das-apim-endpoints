namespace SFA.DAS.Reservations.InnerApi.Responses
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
    }
}