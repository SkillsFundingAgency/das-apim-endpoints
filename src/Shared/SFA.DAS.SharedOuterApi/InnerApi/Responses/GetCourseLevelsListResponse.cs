using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetCourseLevelsListResponse
    {
        [JsonProperty("Levels")]
        public IEnumerable<GetCourseLevelsListItem> Levels { get; set; }
    }

    public class GetCourseLevelsListItem
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}