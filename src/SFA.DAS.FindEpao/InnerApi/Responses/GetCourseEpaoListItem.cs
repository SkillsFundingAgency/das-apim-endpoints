using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetCourseEpaoListItem
    {
        [JsonProperty("endPointAssessorOrganisationId")]
        public string EpaoId { get; set; }
        [JsonProperty("endPointAssessorName")]
        public string Name { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        [JsonProperty("deliveryAreasDetails")]
        public IEnumerable<EpaoDeliveryArea> DeliveryAreas { get; set; }
        [JsonProperty("organisationStandard")]
        public CourseEpaoDetails CourseEpaoDetails { get; set; }
    }
}