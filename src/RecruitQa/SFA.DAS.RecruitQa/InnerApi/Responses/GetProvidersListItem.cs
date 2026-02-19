namespace SFA.DAS.RecruitQa.InnerApi.Responses
{
    public record GetProvidersListItem
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string ContactUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int ProviderTypeId { get; set; }
        public int StatusId { get; set; }
        public GetProvidersListItemAddress Address { get; set; }
    }
}