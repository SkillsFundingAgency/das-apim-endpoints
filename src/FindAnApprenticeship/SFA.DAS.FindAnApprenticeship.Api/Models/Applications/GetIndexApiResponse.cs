using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetIndexApiResponse
    {
        public string VacancyReference { get; set; }
        public string VacancyTitle { get; set; }
        public ApprenticeshipTypes ApprenticeshipType { get; set; }
        public string EmployerName { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool IsMigrated { get; set; }
        public bool IsDisabilityConfident { get; set; }
        public bool IsApplicationComplete { get; set; }
        public EducationHistorySection EducationHistory { get; set; }
        public WorkHistorySection WorkHistory { get; set; }
        public ApplicationQuestionsSection ApplicationQuestions { get; set; }
        public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
        public DisabilityConfidenceSection DisabilityConfidence { get; set; }
        public PreviousApplicationDetails PreviousApplication { get; set; }
        public EmploymentLocationSection? EmploymentLocation { get; set; }

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
        
        public record EmploymentLocationSection : LocationDto
        {
            public string EmploymentLocationStatus { get; set; }
            public static implicit operator EmploymentLocationSection(GetIndexQueryResult.EmploymentLocationSection? source)
            {
                if (source is null) return null;

                return new EmploymentLocationSection
                {
                    Id = source.Id,
                    Addresses = source.Addresses,
                    EmploymentLocationInformation = source.EmploymentLocationInformation,
                    EmployerLocationOption = source.EmployerLocationOption,
                    EmploymentLocationStatus = source.EmploymentLocationStatus
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
            public string? AdditionalQuestion1Label { get; set; }
            public string? AdditionalQuestion2Label { get; set; }
            public Guid? AdditionalQuestion1Id { get; set; }
            public Guid? AdditionalQuestion2Id { get; set; }

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
                ApprenticeshipType = source.ApprenticeshipType,
                EmployerName = source.EmployerName,
                ClosingDate = source.ClosingDate,
                ClosedDate = source.ClosedDate,
                IsMigrated = source.IsMigrated,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsApplicationComplete = source.IsApplicationComplete,
                EducationHistory = source.EducationHistory,
                EmploymentLocation = source.EmploymentLocation,
                WorkHistory = source.WorkHistory,
                ApplicationQuestions = source.ApplicationQuestions,
                InterviewAdjustments = source.InterviewAdjustments,
                DisabilityConfidence = source.DisabilityConfidence,
                PreviousApplication = source.PreviousApplication,
            };
        }

        public class PreviousApplicationDetails
        {
            public string VacancyTitle { get; set; }
            public string EmployerName { get; set; }
            public DateTime SubmissionDate { get; set; }

            public static implicit operator PreviousApplicationDetails(
                GetIndexQueryResult.PreviousApplicationDetails source)
            {
                if (source == null) return null;

                return new PreviousApplicationDetails
                {
                    EmployerName = source.EmployerName,
                    SubmissionDate = source.SubmissionDate,
                    VacancyTitle = source.VacancyTitle
                };
            }
        }

        public record AddressDto(
            string? AddressLine1,
            string? AddressLine2,
            string? AddressLine3,
            string? AddressLine4,
            string? Postcode,
            double? Latitude = null,
            double? Longitude = null)
        {
            public static AddressDto? From(SFA.DAS.SharedOuterApi.Models.Address? source)
            {
                if (source is null)
                {
                    return null;
                }
                
                return new AddressDto(
                    source.AddressLine1,
                    source.AddressLine2,
                    source.AddressLine3,
                    source.AddressLine4,
                    source.Postcode,
                    source.Latitude,
                    source.Longitude);
            }
        }
    }
}
