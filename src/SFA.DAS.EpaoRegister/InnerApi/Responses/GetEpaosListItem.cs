using Newtonsoft.Json;

namespace SFA.DAS.EpaoRegister.InnerApi.Responses
{
    public class GetEpaosListItem
    {
        [JsonProperty("endPointAssessorOrganisationId")]
        public string Id { get; set; }
        [JsonProperty("endPointAssessorName")]
        public string Name { get; set; }
        [JsonProperty("endPointAssessorUkprn")]
        public uint Ukprn { get; set; }
    }
}