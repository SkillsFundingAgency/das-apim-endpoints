using Newtonsoft.Json;

namespace SFA.DAS.EpaoRegister.InnerApi.Responses
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
        [JsonProperty("organisationData")]
        public GetEpaoAddress Address { get; set; }
    }

    public class GetEpaoAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Postcode { get; set; }
    }
}