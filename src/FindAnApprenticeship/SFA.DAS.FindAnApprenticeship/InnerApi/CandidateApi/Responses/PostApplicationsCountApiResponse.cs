using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public record PostApplicationsCountApiResponse
    {
        [JsonProperty("stats")]
        public List<ApplicationStats> Stats { get; init; } = [];

        public record ApplicationStats
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }
    }
}
