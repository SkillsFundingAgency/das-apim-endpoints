using Newtonsoft.Json;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared
{
    public record AddressDto
    {
        [JsonProperty("fullAddress")]
        public string FullAddress { get; set; } = null!;
        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; } = false;
        [JsonProperty("addressOrder")]
        public short AddressOrder { get; set; }
    }
}