using System;
using System.Text.Json.Serialization;

namespace SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi
{

    public class ApprenticeshipBase
    {
        [JsonPropertyName("uln")]
        public long Uln { get; set; }

        [JsonPropertyName("apprenticeshipId")]
        public long ApprenticeshipId { get; set; }

        [JsonPropertyName("apprenticeId")]
        public Guid ApprenticeId { get; set; }

        [JsonPropertyName("employerName")]
        public string EmployerName { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("trainingProviderId")]
        public long TrainingProviderId { get; set; }

        [JsonPropertyName("trainingProviderName")]
        public string TrainingProviderName { get; set; }

        [JsonPropertyName("trainingCode")]
        public string TrainingCode { get; set; }

        [JsonPropertyName("standardUId")]
        public string StandardUId { get; set; }
    }
    public class ApprenticeshipResponse : ApprenticeshipBase
    {
        public long Id { get; set; }
        public long EmployerAccountId { get; set; }
        public long AccountLegalEntityId { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }        
        public DeliveryModel DeliveryModel { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Option { get; set; }        
        public DateTime? EmploymentEndDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? StopDate { get; set; }
        public DateTime? PauseDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool HasHadDataLockSuccess { get; set; }
        public DateTime? OriginalStartDate { get; set; }
        public long ProviderId { get; set; }
        public bool? RecognisePriorLearning { get; set; }
        public int? TrainingTotalHours { get; set; }
        public int? DurationReducedByHours { get; set; }
        public int? DurationReducedBy { get; set; }
        public int? PriceReducedBy { get; set; }
        public int? ApprenticeshipType { get; set; }
    }
}
