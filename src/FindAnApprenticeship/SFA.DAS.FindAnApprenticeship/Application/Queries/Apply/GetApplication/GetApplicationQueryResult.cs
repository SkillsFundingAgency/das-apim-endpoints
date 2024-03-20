using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;

public class GetApplicationQueryResult
{
    public bool IsDisabilityConfident { get; set; }
    public EducationHistorySection EducationHistory { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }


    public Candidate CandidateDetails { get; set; }



    public record Candidate
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CandidateAddress Address { get; set; }
    }

    public record CandidateAddress
    {
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string Postcode { get; set; } = null!;

        public static implicit operator CandidateAddress(GetAddressApiResponse source)
        {
            if (source is null) return null;

            return new CandidateAddress
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
    }

    public class WorkHistorySection
    {
        public string JobsStatus { get; set; }
        public string VolunteeringAndWorkExperienceStatus { get; set; }
    }

    public class ApplicationQuestionsSection
    {
        public string SkillsAndStrengthsStatus { get; set; }
        public string WhatInterestsYouStatus { get; set; }
        public string AdditionalQuestion1Status { get; set; }
        public string AdditionalQuestion2Status { get; set; }
        public Guid? AdditionalQuestion1Id { get; set; }
        public Guid? AdditionalQuestion2Id { get; set; }
    }

    public class InterviewAdjustmentsSection
    {
        public string RequestAdjustmentsStatus { get; set; }
    }
    public class DisabilityConfidenceSection
    {
        public string InterviewUnderDisabilityConfidentStatus { get; set; }
    }
}