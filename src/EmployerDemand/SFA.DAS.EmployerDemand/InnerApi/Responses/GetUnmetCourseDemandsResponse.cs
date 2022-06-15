using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetUnmetCourseDemandsResponse
    {
        [JsonProperty("unmetCourseDemands")]
        public IReadOnlyList<UnmetEmployerCourseDemand> UnmetCourseDemands { get; set; }
    }
}