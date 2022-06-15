using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetVacancyApiResponse : GetVacanciesListItem
    {
        [JsonProperty("longDescription")]
        public string LongDescription { get; set; }

        [JsonProperty("outcomeDescription")]
        public string OutcomeDescription { get; set; }

        [JsonProperty("trainingDescription")]
        public string TrainingDescription { get; set; }

        [JsonProperty("employerDescription")]
        public string EmployerDescription { get; set; }
        
        [JsonProperty("thingsToConsider")]
        public string ThingsToConsider { get; set; }
        
        [JsonProperty("skills")]
        public List<string> Skills { get; set; }

        [JsonProperty("qualifications")]
        public List<GetVacancyQualificationResponseItem> Qualifications { get; set; }
    }

    public class GetVacancyQualificationResponseItem
    {
        [JsonProperty("weighting")]
        public QualificationWeighting Weighting { get ; set ; }
        [JsonProperty("qualificationType")]
        public string QualificationType { get ; set ; }
        [JsonProperty("subject")]
        public string Subject { get ; set ; }
        [JsonProperty("grade")]
        public string Grade { get ; set ; }
    }
    
    public enum QualificationWeighting
    {
        Essential,
        Desired
    }
}