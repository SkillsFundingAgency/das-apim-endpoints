﻿using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class GetWorkHistoriesApiResponse
    {
        [JsonProperty("workHistories")]
        public List<WorkHistoryItem> WorkHistories { get; set; }

        public class WorkHistoryItem
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }
            [JsonProperty("workHistoryType")]
            public WorkHistoryType WorkHistoryType { get; set; }
            [JsonProperty("employer")]
            public string Employer { get; set; }
            [JsonProperty("jobTitle")]
            public string JobTitle { get; set; }
            [JsonProperty("startDate")]
            public DateTime StartDate { get; set; }
            [JsonProperty("endDate")]
            public DateTime? EndDate { get; set; }
            [JsonProperty("applicationId")]
            public Guid ApplicationId { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
        }
    }
}