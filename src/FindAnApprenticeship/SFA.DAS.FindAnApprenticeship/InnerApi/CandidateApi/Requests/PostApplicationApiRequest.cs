using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Extensions.LegacyApi;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
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

            public static LegacyApplication Map(GetLegacyApplicationsByEmailApiResponse.Application source,
                IVacancy vacancy, Guid candidateId)
            {
                return new LegacyApplication
                {
                    CandidateId = candidateId,
                    VacancyReference =
                        source.Vacancy.VacancyReference.Replace("VAC", "",
                            StringComparison.CurrentCultureIgnoreCase),
                    Status = source.Status.ToFaaApplicationStatus(),
                    DateApplied = source.DateApplied,
                    SuccessfulDateTime = source.SuccessfulDateTime,
                    UnsuccessfulDateTime = source.UnsuccessfulDateTime,
                    SkillsAndStrengths = source.CandidateInformation.AboutYou.Strengths,
                    Support = source.CandidateInformation.AboutYou.Support,
                    AdditionalQuestion1Answer = source.AdditionalQuestion1Answer,
                    AdditionalQuestion2Answer = source.AdditionalQuestion2Answer,
                    AdditionalQuestion1 = vacancy.AdditionalQuestion1,
                    AdditionalQuestion2 = vacancy.AdditionalQuestion2,
                    IsDisabilityConfident = vacancy.IsDisabilityConfident,
                    Qualifications = source.CandidateInformation.Qualifications.Select(x => new Qualification
                    {
                        Grade = x.Grade,
                        IsPredicted = x.IsPredicted,
                        QualificationType = x.QualificationType,
                        Subject = x.Subject,
                        Year = x.Year
                    }).ToList(),
                    TrainingCourses = source.CandidateInformation.TrainingCourses.Select(x => new TrainingCourse
                    {
                        FromDate = x.FromDate,
                        Provider = x.Provider,
                        Title = x.Title,
                        ToDate = x.ToDate
                    }).ToList(),
                    WorkExperience = source.CandidateInformation.WorkExperiences.Select(x => new WorkExperienceItem
                    {
                        Description = x.Description,
                        Employer = x.Employer,
                        FromDate = x.FromDate,
                        ToDate = x.ToDate,
                        JobTitle = x.JobTitle
                    }).ToList()
                };
            }
        }
    }
}
