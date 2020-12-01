using Newtonsoft.Json;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetEpaosListItem
    {
        [JsonProperty("endPointAssessorOrganisationId")]
        public string Id { get; set; }
        [JsonProperty("endPointAssessorName")]
        public string Name { get; set; }
    }
}