namespace SFA.DAS.VacanciesManage.InnerApi.Responses
{
    public class GetProvidersListItem
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
    public class GetProvidersListItemAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
    }
}
