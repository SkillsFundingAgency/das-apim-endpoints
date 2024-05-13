﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
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
            public Guid CandidateId { get; set; }
            public string VacancyReference { get; set; }
            public ApplicationStatus Status { get; set; }
            public IEnumerable<string> AdditionalQuestions { get; set; }
            public short IsAdditionalQuestion1Complete { get; set; }
            public short IsAdditionalQuestion2Complete { get; set; }
            public short IsDisabilityConfidenceComplete { get; set; }

            public List<Qualification> Qualifications { get; set; }
            public List<TrainingCourse> TrainingCourses { get; set; }
            public List<WorkExperienceItem> WorkExperience { get; set; }

            public static PostApplicationApiRequestData Map(GetLegacyApplicationsByEmailApiResponse.Application source, GetApprenticeshipVacancyItemResponse vacancy, Guid candidateId)
            {
                var additionalQuestions = new List<string>();
                if (vacancy.AdditionalQuestion1 != null) { additionalQuestions.Add(vacancy.AdditionalQuestion1); }
                if (vacancy.AdditionalQuestion2 != null) { additionalQuestions.Add(vacancy.AdditionalQuestion2); }

                return new PostApplicationApiRequestData
                {
                    CandidateId = candidateId,
                    VacancyReference =
                        source.Vacancy.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase),
                    Status = ApplicationStatus.Draft, //todo: map this
                    AdditionalQuestions = additionalQuestions,
                    IsAdditionalQuestion1Complete =
                        string.IsNullOrEmpty(vacancy.AdditionalQuestion1) ? (short)4 : (short)0,
                    IsAdditionalQuestion2Complete =
                        string.IsNullOrEmpty(vacancy.AdditionalQuestion2) ? (short)4 : (short)0,
                    IsDisabilityConfidenceComplete = vacancy.IsDisabilityConfident ? (short)0 : (short)4,
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
