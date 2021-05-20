using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetExpiredShortlistsResponse
    {
        [JsonProperty("userIds")]
        public List<Guid> UserIds { get; set; }
    }
}