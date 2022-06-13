using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetTraineeshipVacancyApiResponse : GetTraineeshipVacanciesListItem
    {
        [JsonProperty("longDescription")]
        public string LongDescription { get; set; }

        [JsonProperty("outcomeDescription")]
        public string OutcomeDescription { get; set; }

        [JsonProperty("WorkExperience")]
        public string WorkExperience { get; set; }

        [JsonProperty("thingsToConsider")]
        public string ThingsToConsider { get; set; }

        [JsonProperty("skills")]
        public List<string> Skills { get; set; }
    }
}