using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetApplicationApiResponse
{
    public bool IsDisabilityConfident { get; set; }
    public EducationHistorySection EducationHistory { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }

    public CandidateDetailsSection Candidate { get; set; }
   
    public static implicit operator GetApplicationApiResponse(GetApplicationQueryResult source)
    {
        return new GetApplicationApiResponse
        {
            Candidate = source.CandidateDetails,
            DisabilityConfidence = source.DisabilityConfidence,
            ApplicationQuestions = source.ApplicationQuestions,
            EducationHistory = source.EducationHistory,
            InterviewAdjustments = source.InterviewAdjustments,
            IsDisabilityConfident = source.IsDisabilityConfident,
            WorkHistory = source.WorkHistory
        };
    }

    public class CandidateDetailsSection
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public AddressDetailsSection Address { get; set; }

        public static implicit operator CandidateDetailsSection(GetApplicationQueryResult.Candidate source)
        {
            if (source is null) return null;

            return new CandidateDetailsSection
            {
                Id = source.Id,
                GovUkIdentifier = source.GovUkIdentifier,
                Email = source.Email,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                PhoneNumber = source.PhoneNumber,
                DateOfBirth = source.DateOfBirth,
                Address = source.Address
            };
        }
    }

    public class AddressDetailsSection
    {
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string Postcode { get; set; } = null!;

        public static implicit operator AddressDetailsSection(GetApplicationQueryResult.CandidateAddress source)
        {
            if (source is null) return null;

            return new AddressDetailsSection
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                Town = source.Town,
                County = source.County,
                Postcode = source.Postcode,
            };
        }
    }

    public class EducationHistorySection
    {
        public string QualificationsStatus { get; set; }
        public string TrainingCoursesStatus { get; set; }

        public static implicit operator EducationHistorySection(GetApplicationQueryResult.EducationHistorySection source)
        {
            return new EducationHistorySection
            {
                QualificationsStatus = source.QualificationsStatus,
                TrainingCoursesStatus = source.TrainingCoursesStatus
            };
        }
    }

    public class WorkHistorySection
    {
        public string JobsStatus { get; set; }
        public string VolunteeringAndWorkExperienceStatus { get; set; }

        public static implicit operator WorkHistorySection(GetApplicationQueryResult.WorkHistorySection source)
        {
            return new WorkHistorySection
            {
                JobsStatus = source.JobsStatus,
                VolunteeringAndWorkExperienceStatus = source.VolunteeringAndWorkExperienceStatus
            };
        }
    }

    public class ApplicationQuestionsSection
    {
        public string SkillsAndStrengthsStatus { get; set; }
        public string WhatInterestsYouStatus { get; set; }
        public string AdditionalQuestion1Status { get; set; }
        public string AdditionalQuestion2Status { get; set; }
        public Guid? AdditionalQuestion1Id { get; set; }
        public Guid? AdditionalQuestion2Id { get; set; }

        public static implicit operator ApplicationQuestionsSection(GetApplicationQueryResult.ApplicationQuestionsSection source)
        {
            return new ApplicationQuestionsSection
            {
                SkillsAndStrengthsStatus = source.SkillsAndStrengthsStatus,
                WhatInterestsYouStatus = source.WhatInterestsYouStatus,
                AdditionalQuestion1Status = source.AdditionalQuestion1Status,
                AdditionalQuestion2Status = source.AdditionalQuestion2Status,
                AdditionalQuestion1Id = source.AdditionalQuestion1Id,
                AdditionalQuestion2Id = source.AdditionalQuestion2Id
            };
        }
    }

    public class InterviewAdjustmentsSection
    {
        public string RequestAdjustmentsStatus { get; set; }

        public static implicit operator InterviewAdjustmentsSection(GetApplicationQueryResult.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                RequestAdjustmentsStatus = source.RequestAdjustmentsStatus
            };
        }
    }

    public class DisabilityConfidenceSection
    {
        public string InterviewUnderDisabilityConfidentStatus { get; set; }

        public static implicit operator DisabilityConfidenceSection(GetApplicationQueryResult.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfidentStatus = source.InterviewUnderDisabilityConfidentStatus
            };
        }
    }
}