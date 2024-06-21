using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class GetWorkHistoriesApiResponse
    {
        [JsonProperty("workHistories")]
        public List<GetWorkHistoryItemApiResponse> WorkHistories { get; set; }

        
    }
}
