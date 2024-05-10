using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses
{
    public class GetLegacyApplicationsByEmailApiResponse
    {
        [JsonProperty("apprenticeships")]
        public List<Application> Applications { get; set; }

        public class Application
        {
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
            [JsonProperty("workExperience")]
            public List<WorkExperience> WorkExperiences { get; set; } = null!;
            [JsonProperty("aboutYou")]
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
            [JsonProperty("employer")]
            public string Employer { get; set; }

            [JsonProperty("jobTitle")]
            public string JobTitle { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("fromDate")]
            public DateTime FromDate { get; set; }

            [JsonProperty("toDate")]
            public DateTime ToDate { get; set; }
        }

        public class TrainingCourse
        {
            [JsonProperty("provider")]
            public string Provider { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("fromDate")]
            public DateTime FromDate { get; set; }
            [JsonProperty("toDate")]
            public DateTime ToDate { get; set; }
        }

        public class AboutYou
        {
            [JsonProperty("strengths")]
            public string Strengths { get; set; }

            [JsonProperty("improvements")]
            public string Improvements { get; set; }

            [JsonProperty("hobbiesAndInterests")]
            public string HobbiesAndInterests { get; set; }

            [JsonProperty("support")]
            public string Support { get; set; }
        }
    }
}
