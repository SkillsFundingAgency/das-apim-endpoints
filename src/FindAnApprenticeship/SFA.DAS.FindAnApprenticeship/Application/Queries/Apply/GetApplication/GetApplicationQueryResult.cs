using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;

public record GetApplicationQueryResult
{
    public bool IsDisabilityConfident { get; set; }
    public Candidate CandidateDetails { get; set; }
    public AboutYouSection AboutYou { get; set; }
    public EducationHistorySection EducationHistory { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }
    public WhatIsYourInterestSection WhatIsYourInterest { get; set; }
    public bool IsApplicationComplete { get; set; }


    public record Candidate
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
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

    public record EducationHistorySection
    {
        public string QualificationsStatus { get; set; }
        public string TrainingCoursesStatus { get; set; }
        public List<TrainingCourse> TrainingCourses { get; set; } = [];
        public record TrainingCourse
        {
            public Guid Id { get; set; }
            public string CourseName { get; set; }
            public int YearAchieved { get; set; }

            public static implicit operator TrainingCourse(GetTrainingCoursesApiResponse.TrainingCourseItem source)
            {
                return new TrainingCourse
                {
                    Id = source.Id,
                    CourseName = source.CourseName,
                    YearAchieved = source.YearAchieved
                };
            }
        }
    }

    public record WorkHistorySection
    {
        public string JobsStatus { get; set; }
        public string VolunteeringAndWorkExperienceStatus { get; set; }
        public List<Job> Jobs { get; set; } = [];
        public List<VolunteeringAndWorkExperience> VolunteeringAndWorkExperiences { get; set; } = [];
        public record Job
        {
            public Guid Id { get; set; }
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }

            public static implicit operator Job(GetWorkHistoriesApiResponse.WorkHistoryItem source)
            {
                return new Job
                {
                    Id = source.Id,
                    Employer = source.Employer,
                    JobTitle = source.JobTitle,
                    StartDate = source.StartDate,
                    EndDate = source.EndDate,
                    Description = source.Description
                };
            }
        }
        public record VolunteeringAndWorkExperience
        {
            public Guid Id { get; set; }
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }

            public static implicit operator VolunteeringAndWorkExperience(GetWorkHistoriesApiResponse.WorkHistoryItem source)
            {
                return new VolunteeringAndWorkExperience
                {
                    Id = source.Id,
                    Employer = source.Employer,
                    JobTitle = source.JobTitle,
                    StartDate = source.StartDate,
                    EndDate = source.EndDate,
                    Description = source.Description
                };
            }
        }
    }

    public record ApplicationQuestionsSection
    {
        public string SkillsAndStrengthsStatus { get; set; }
        public string WhatInterestsYouStatus { get; set; }
        public Question? AdditionalQuestion1 { get; set; }
        public Question? AdditionalQuestion2 { get; set; }

        public record Question
        {
            public Guid Id { get; set; }
            public string Status { get; set; }
            public string QuestionLabel { get; set; }
            public string Answer { get; set; }
        }
    }

    public record InterviewAdjustmentsSection
    {
        public string RequestAdjustmentsStatus { get; set; }
        public string InterviewAdjustmentsDescription { get; set; }
    }
    public record DisabilityConfidenceSection
    {
        public string InterviewUnderDisabilityConfidentStatus { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
    }

    public record WhatIsYourInterestSection
    {
        public string WhatIsYourInterest { get; set; }
    }

    public record AboutYouSection
    {
        public string SkillsAndStrengths { get; set; }
        public string Improvements { get; set; }
        public string HobbiesAndInterests { get; set; }
        public string Support { get; set; }
    }
}