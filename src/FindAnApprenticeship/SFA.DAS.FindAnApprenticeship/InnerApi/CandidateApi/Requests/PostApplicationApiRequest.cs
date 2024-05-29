using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Extensions.LegacyApi;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PostApplicationApiRequest(PostApplicationApiRequest.PostApplicationApiRequestData data)
        : IPostApiRequest
    {
        public object Data { get; set; } = data;

        public string PostUrl => $"api/applications";

        public class PostApplicationApiRequestData
        {
            public LegacyApplication LegacyApplication { get; set; }
        }

        public class LegacyApplication
        {
            public Guid CandidateId { get; set; }
            public string VacancyReference { get; set; }
            public ApplicationStatus Status { get; set; }
            public DateTime? DateApplied { get; set; }
            public DateTime? SuccessfulDateTime { get; set; }
            public DateTime? UnsuccessfulDateTime { get; set; }
            public string UnsuccessfulReason { get; set; }
            public string AdditionalQuestion1 { get; set; }
            public string AdditionalQuestion2 { get; set; }
            public string AdditionalQuestion1Answer { get; set; }
            public string AdditionalQuestion2Answer { get; set; }
            public bool IsDisabilityConfident { get; set; }
            public List<Qualification> Qualifications { get; set; }
            public List<TrainingCourse> TrainingCourses { get; set; }
            public List<WorkExperienceItem> WorkExperience { get; set; }
            public string SkillsAndStrengths { get; set; }
            public string Support { get; set; }

            public class Qualification
            {
                [JsonPropertyName("qualificationType")]
                public string QualificationType { get; set; }
                [JsonPropertyName("subject")]
                public string Subject { get; set; }
                [JsonPropertyName("grade")]
                public string Grade { get; set; }
                [JsonPropertyName("isPredicted")]
                public bool IsPredicted { get; set; }
                [JsonPropertyName("year")]
                public int Year { get; set; }
            }
            public class WorkExperienceItem
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
        }
    }
}
