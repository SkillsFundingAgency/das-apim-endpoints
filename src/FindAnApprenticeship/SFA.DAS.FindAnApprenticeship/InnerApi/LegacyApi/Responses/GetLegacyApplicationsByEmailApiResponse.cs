using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses
{
    public class GetLegacyApplicationsByEmailApiResponse
    {
        [JsonPropertyName("apprenticeships")]
        public List<Application> Applications { get; set; }

        public class Application
        {
            public ApplicationStatus Status { get; set; }
            public DateTime? DateApplied { get; set; }
            public DateTime? SuccessfulDateTime { get; set; }
            public DateTime? UnsuccessfulDateTime { get; set; }
            public string AdditionalQuestion1Answer { get; set; }
            public string AdditionalQuestion2Answer { get; set; }

            public Vacancy Vacancy { get; set; }
            public CandidateInformation CandidateInformation { get; set; }
        }

        public class Vacancy
        {
            public long Id { get; set; }
            public string VacancyReference { get; set; }
        }

        public class CandidateInformation
        {
            public List<Qualification> Qualifications { get; set; } = null!;
            [JsonPropertyName("workExperience")]
            public List<WorkExperience> WorkExperiences { get; set; } = null!;
            [JsonPropertyName("trainingCourses")]
            public List<TrainingCourse> TrainingCourses { get; set; } = null!;
            [JsonPropertyName("aboutYou")]
            public AboutYou AboutYou { get; set; }

        }

        public class Qualification
        {
            public string QualificationType { get; set; }
            public string Subject { get; set; }
            public string Grade { get; set; }
            public bool IsPredicted { get; set; }
            public int Year { get; set; }
        }
        public class WorkExperience
        {
            [JsonPropertyName("employer")]
            public string Employer { get; set; }

            [JsonPropertyName("jobTitle")]
            public string JobTitle { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("fromDate")]
            public DateTime FromDate { get; set; }

            [JsonPropertyName("toDate")]
            public DateTime ToDate { get; set; }
        }

        public class TrainingCourse
        {
            [JsonPropertyName("provider")]
            public string Provider { get; set; }
            [JsonPropertyName("title")]
            public string Title { get; set; }
            [JsonPropertyName("fromDate")]
            public DateTime FromDate { get; set; }
            [JsonPropertyName("toDate")]
            public DateTime ToDate { get; set; }
        }

        public class AboutYou
        {
            [JsonPropertyName("strengths")]
            public string Strengths { get; set; }

            [JsonPropertyName("improvements")]
            public string Improvements { get; set; }

            [JsonPropertyName("hobbiesAndInterests")]
            public string HobbiesAndInterests { get; set; }

            [JsonPropertyName("support")]
            public string Support { get; set; }
        }
    }
}
