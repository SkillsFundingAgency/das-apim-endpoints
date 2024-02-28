using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetIndexApiResponse
    {
        public string VacancyReference { get; set; }
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public DateTime ClosingDate { get; set; }
        public bool IsDisabilityConfident { get; set; }

        public EducationHistorySection EducationHistory { get; set; }
        public WorkHistorySection WorkHistory { get; set; }
        public ApplicationQuestionsSection ApplicationQuestions { get; set; }
        public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
        public DisabilityConfidenceSection DisabilityConfidence { get; set; }

        public class EducationHistorySection
        {
            public string Qualifications { get; set; }
            public string TrainingCourses { get; set; }

            public static implicit operator EducationHistorySection(GetIndexQueryResult.EducationHistorySection source)
            {
                return new EducationHistorySection
                {
                    Qualifications = source.Qualifications,
                    TrainingCourses = source.TrainingCourses
                };
            }
        }

        public class WorkHistorySection
        {
            public string Jobs { get; set; }
            public string VolunteeringAndWorkExperience { get; set; }

            public static implicit operator WorkHistorySection(GetIndexQueryResult.WorkHistorySection source)
            {
                return new WorkHistorySection
                {
                    Jobs = source.Jobs,
                    VolunteeringAndWorkExperience = source.VolunteeringAndWorkExperience
                };
            }
        }

        public class ApplicationQuestionsSection
        {
            public string SkillsAndStrengths { get; set; }
            public string WhatInterestsYou { get; set; }
            public string AdditionalQuestion1 { get; set; }
            public string AdditionalQuestion2 { get; set; }
            public string AdditionalQuestion1Label { get; set; }
            public string AdditionalQuestion2Label { get; set; }
            public Guid AdditionalQuestion1Id { get; set; }
            public Guid AdditionalQuestion2Id { get; set; }

            public static implicit operator ApplicationQuestionsSection(GetIndexQueryResult.ApplicationQuestionsSection source)
            {
                return new ApplicationQuestionsSection
                {
                    SkillsAndStrengths = source.SkillsAndStrengths,
                    WhatInterestsYou = source.WhatInterestsYou,
                    AdditionalQuestion1 = source.AdditionalQuestion1,
                    AdditionalQuestion2 = source.AdditionalQuestion2,
                    AdditionalQuestion1Label = source.AdditionalQuestion1Label,
                    AdditionalQuestion2Label = source.AdditionalQuestion2Label,
                    AdditionalQuestion1Id = source.AdditionalQuestion1Id,
                    AdditionalQuestion2Id = source.AdditionalQuestion2Id
                };
            }
        }

        public class InterviewAdjustmentsSection
        {
            public string RequestAdjustments { get; set; }

            public static implicit operator InterviewAdjustmentsSection(GetIndexQueryResult.InterviewAdjustmentsSection source)
            {
                return new InterviewAdjustmentsSection
                {
                    RequestAdjustments = source.RequestAdjustments
                };
            }
        }
        public class DisabilityConfidenceSection
        {
            public string InterviewUnderDisabilityConfident { get; set; }

            public static implicit operator DisabilityConfidenceSection(GetIndexQueryResult.DisabilityConfidenceSection source)
            {
                return new DisabilityConfidenceSection
                {
                    InterviewUnderDisabilityConfident = source.InterviewUnderDisabilityConfident
                };
            }
        }

        public static implicit operator GetIndexApiResponse(GetIndexQueryResult source)
        {
            return new GetIndexApiResponse
            {
                VacancyReference = source.VacancyReference,
                VacancyTitle = source.VacancyTitle,
                EmployerName = source.EmployerName,
                ClosingDate = source.ClosingDate,
                IsDisabilityConfident = source.IsDisabilityConfident,
                EducationHistory = source.EducationHistory,
                WorkHistory = source.WorkHistory,
                ApplicationQuestions = source.ApplicationQuestions,
                InterviewAdjustments = source.InterviewAdjustments,
                DisabilityConfidence = source.DisabilityConfidence
            };
        }
    }
}
