using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetUnmetCourseDemandsResponse
    {
        [JsonProperty("employerDemandIds")]
        public List<Guid> EmployerDemandIds { get; set; }
    }
}