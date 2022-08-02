using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetCourseEpaoListItem
    {
        [JsonPropertyName("endPointAssessorOrganisationId")]
        public string EpaoId { get; set; }
        [JsonPropertyName("endPointAssessorName")]
        public string Name { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        [JsonPropertyName("deliveryAreasDetails")]
        public IEnumerable<EpaoDeliveryArea> DeliveryAreas { get; set; }
        [JsonPropertyName("organisationStandard")]
        public CourseEpaoDetails CourseEpaoDetails { get; set; }
    }
}