using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public record GetApplicationApiResponse
{
    public bool IsDisabilityConfident { get; set; }
    public bool IsApplicationComplete { get; set; }
    public CandidateDetailsSection Candidate { get; set; }
    public AboutYouSection AboutYou { get; set; }
    public EducationHistorySection EducationHistory { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }
    public WhatIsYourInterestSection WhatIsYourInterest { get; set; }

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
            IsApplicationComplete = source.IsApplicationComplete,
            WorkHistory = source.WorkHistory,
            WhatIsYourInterest = source.WhatIsYourInterest,
            AboutYou = source.AboutYou,
        };
    }

    public record CandidateDetailsSection
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
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
                Address = source.Address
            };
        }
    }

    public record AddressDetailsSection
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

    public record EducationHistorySection
    {
        public string QualificationsStatus { get; set; }
        public string TrainingCoursesStatus { get; set; }
        public List<TrainingCourse> TrainingCourses { get; set; } = [];

        public static implicit operator EducationHistorySection(GetApplicationQueryResult.EducationHistorySection source)
        {
            return new EducationHistorySection
            {
                QualificationsStatus = source.QualificationsStatus,
                TrainingCoursesStatus = source.TrainingCoursesStatus,
                TrainingCourses = source.TrainingCourses.Select(x => (GetApplicationApiResponse.EducationHistorySection.TrainingCourse)x).ToList()
            };
        }
        
        public record TrainingCourse
        {
            public Guid Id { get; set; }
            public string CourseName { get; set; }
            public int YearAchieved { get; set; }
            public static implicit operator TrainingCourse(GetApplicationQueryResult.EducationHistorySection.TrainingCourse source)
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

        public static implicit operator WorkHistorySection(GetApplicationQueryResult.WorkHistorySection source)
        {
            return new WorkHistorySection
            {
                JobsStatus = source.JobsStatus,
                VolunteeringAndWorkExperienceStatus = source.VolunteeringAndWorkExperienceStatus,
                Jobs = source.Jobs.Select(x => (GetApplicationApiResponse.WorkHistorySection.Job)x).ToList(),
                VolunteeringAndWorkExperiences = source.VolunteeringAndWorkExperiences.Select(x => (GetApplicationApiResponse.WorkHistorySection.VolunteeringAndWorkExperience)x).ToList(),
            };
        }
        public record Job
        {
            public Guid Id { get; set; }
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }

            public static implicit operator Job(GetApplicationQueryResult.WorkHistorySection.Job source)
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

            public static implicit operator VolunteeringAndWorkExperience(GetApplicationQueryResult.WorkHistorySection.VolunteeringAndWorkExperience source)
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

            public static implicit operator Question(GetApplicationQueryResult.ApplicationQuestionsSection.Question source)
            {
                if (source == null)
                {
                    return null;
                }
                return new Question
                {
                    Id = source.Id,
                    Answer = source.Answer,
                    QuestionLabel = source.QuestionLabel,
                    Status = source.Status,
                };
            }
        }

        public static implicit operator ApplicationQuestionsSection(GetApplicationQueryResult.ApplicationQuestionsSection source)
        {
            return new ApplicationQuestionsSection
            {
                SkillsAndStrengthsStatus = source.SkillsAndStrengthsStatus,
                WhatInterestsYouStatus = source.WhatInterestsYouStatus,
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
            };
        }
    }

    public record InterviewAdjustmentsSection
    {
        public string RequestAdjustmentsStatus { get; set; }
        public string InterviewAdjustmentsDescription { get; set; }

        public static implicit operator InterviewAdjustmentsSection(GetApplicationQueryResult.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                RequestAdjustmentsStatus = source.RequestAdjustmentsStatus,
                InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription
            };
        }
    }

    public record DisabilityConfidenceSection
    {
        public string InterviewUnderDisabilityConfidentStatus { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }

        public static implicit operator DisabilityConfidenceSection(GetApplicationQueryResult.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfidentStatus = source.InterviewUnderDisabilityConfidentStatus,
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme
            };
        }
    }

    public record WhatIsYourInterestSection
    {
        public string WhatIsYourInterest { get; set; }
        public static implicit operator WhatIsYourInterestSection(GetApplicationQueryResult.WhatIsYourInterestSection source)
        {
            return new WhatIsYourInterestSection
            {
                WhatIsYourInterest = source.WhatIsYourInterest
            };
        }
    }

    public record AboutYouSection
    {
        public string SkillsAndStrengths { get; set; }
        public string Improvements { get; set; }
        public string HobbiesAndInterests { get; set; }
        public string Support { get; set; }

        public static implicit operator AboutYouSection(GetApplicationQueryResult.AboutYouSection source)
        {
            return new AboutYouSection
            {
                Support = source.Support,
                HobbiesAndInterests = source.HobbiesAndInterests,
                Improvements = source.Improvements,
                SkillsAndStrengths = source.SkillsAndStrengths
            };
        }

    }
}