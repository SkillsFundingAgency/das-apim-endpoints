using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
{
    public class GetVacancyApiResponse : GetVacanciesListItem
    {
        [JsonPropertyName("longDescription")]
        public string LongDescription { get; set; }

        [JsonPropertyName("outcomeDescription")]
        public string OutcomeDescription { get; set; }

        [JsonPropertyName("trainingDescription")]
        public string TrainingDescription { get; set; }

        [JsonPropertyName("employerDescription")]
        public string EmployerDescription { get; set; }
        
        [JsonPropertyName("thingsToConsider")]
        public string ThingsToConsider { get; set; }
        
        [JsonPropertyName("skills")]
        public List<string> Skills { get; set; }

        [JsonPropertyName("qualifications")]
        public List<GetVacancyQualificationResponseItem> Qualifications { get; set; }
    }

    public class GetVacancyQualificationResponseItem
    {
        [JsonPropertyName("weighting")]
        public QualificationWeighting Weighting { get ; set ; }
        [JsonPropertyName("qualificationType")]
        public string QualificationType { get ; set ; }
        [JsonPropertyName("subject")]
        public string Subject { get ; set ; }
        [JsonPropertyName("grade")]
        public string Grade { get ; set ; }
    }
    
    public enum QualificationWeighting
    {
        Essential,
        Desired
    }
}