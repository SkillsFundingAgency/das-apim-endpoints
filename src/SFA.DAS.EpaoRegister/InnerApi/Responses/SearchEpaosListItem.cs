namespace SFA.DAS.EpaoRegister.InnerApi.Responses
{
    public class SearchEpaosListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint Ukprn { get; set; }
        public string Email { get; set; }
        public SearchEpaosAddress Address { get; set; }
    }

    public class SearchEpaosAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}