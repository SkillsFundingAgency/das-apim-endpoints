using Newtonsoft.Json;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetEpaoResponse
    {
        [JsonProperty("organisationId")]
        public string Id { get; set; }
        public string Name { get; set; }
        public uint? Ukprn { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int? OrganisationTypeId { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactName { get; set; }
        public GetEpaoOrganisationData OrganisationData { get; set; }
    }

    public class GetEpaoOrganisationData
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string WebsiteLink { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Postcode { get; set; }
    }
}