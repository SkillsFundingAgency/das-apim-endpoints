﻿using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetIndexApiResponse
    {
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
            public string Travel { get; set; }

            public static implicit operator ApplicationQuestionsSection(GetIndexQueryResult.ApplicationQuestionsSection source)
            {
                return new ApplicationQuestionsSection
                {
                    SkillsAndStrengths = source.SkillsAndStrengths,
                    WhatInterestsYou = source.WhatInterestsYou,
                    Travel = source.Travel
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
