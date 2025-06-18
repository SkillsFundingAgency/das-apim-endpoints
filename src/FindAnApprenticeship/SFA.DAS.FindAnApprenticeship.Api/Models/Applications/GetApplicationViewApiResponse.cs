using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplication;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public record GetApplicationViewApiResponse
{
    public AboutYouSection AboutYou { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public ApprenticeshipTypes? ApprenticeshipType { get; set; } = ApprenticeshipTypes.Standard;
    public bool IsDisabilityConfident { get; set; }
    public CandidateDetailsSection Candidate { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }
    public EducationHistorySection EducationHistory { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public VacancyDetailsSection VacancyDetails { get; set; }
    public WhatIsYourInterestSection WhatIsYourInterest { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public EmploymentLocationSection? EmploymentLocation { get; set; }
    public string ApplicationStatus { get; set; }
    public DateTime? WithdrawnDate { get; set; }
    public DateTime? MigrationDate { get; set; }

    public static implicit operator GetApplicationViewApiResponse(GetApplicationViewQueryResult source)
    {
        return new GetApplicationViewApiResponse
        {
            Candidate = source.CandidateDetails,
            DisabilityConfidence = source.DisabilityConfidence,
            ApplicationQuestions = source.ApplicationQuestions,
            EducationHistory = source.EducationHistory,
            InterviewAdjustments = source.InterviewAdjustments,
            IsDisabilityConfident = source.IsDisabilityConfident,
            WorkHistory = source.WorkHistory,
            WhatIsYourInterest = source.WhatIsYourInterest,
            AboutYou = source.AboutYou,
            VacancyDetails = source.VacancyDetails,
            ApplicationStatus = source.ApplicationStatus,
            WithdrawnDate = source.WithdrawnDate,
            MigrationDate = source.MigrationDate,
            ApprenticeshipType = source.ApprenticeshipType,
            EmploymentLocation = source.EmploymentLocation,
        };
    }

    public record VacancyDetailsSection
    {
        public string Title { get; set; }
        public string EmployerName { get; set; }

        public static implicit operator VacancyDetailsSection(GetApplicationViewQueryResult.VacancyDetailsSection source)
        {
            if (source is null) return null;

            return new VacancyDetailsSection
            {
                Title = source.Title,
                EmployerName = source.EmployerName
            };
        }
    }

    public record EmploymentLocationSection : LocationDto
    {
        public static implicit operator EmploymentLocationSection(GetApplicationViewQueryResult.EmploymentLocationSection? source)
        {
            if (source is null) return null;
            return new EmploymentLocationSection
            {
                Id = source.Id,
                EmploymentLocationInformation = source.EmploymentLocationInformation,
                Addresses = source.Addresses,
                EmployerLocationOption = source.EmployerLocationOption,
            };
        }
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

        public static implicit operator CandidateDetailsSection(GetApplicationViewQueryResult.Candidate source)
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

        public static implicit operator AddressDetailsSection(GetApplicationViewQueryResult.CandidateAddress source)
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
        public List<TrainingCourse> TrainingCourses { get; set; } = [];
        public List<Qualification> Qualifications { get; set; } = [];
        public List<QualificationReference> QualificationTypes { get; set; } = [];

        public static implicit operator EducationHistorySection(GetApplicationViewQueryResult.EducationHistorySection source)
        {
            return new EducationHistorySection
            {
                TrainingCourses = source.TrainingCourses.Select(x => (TrainingCourse)x).ToList(),
                Qualifications = source.Qualifications.Select(x => (Qualification)x).ToList(),
                QualificationTypes = source.QualificationTypes.Select(x => (QualificationReference)x).ToList()
            };
        }
        
        public record TrainingCourse
        {
            public string CourseName { get; set; }
            public int YearAchieved { get; set; }
            public static implicit operator TrainingCourse(GetApplicationViewQueryResult.EducationHistorySection.TrainingCourse source)
            {
                return new TrainingCourse
                {
                    CourseName = source.CourseName,
                    YearAchieved = source.YearAchieved
                };
            }
        }

        public record Qualification
        {
            public Guid Id { get; set; }
            public string? Subject { get; set; }
            public string? Grade { get; set; }
            public string? AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }
            public Guid QualificationReferenceId { get; set; }
            public short? QualificationOrder { get; set; }
            public QualificationReference QualificationReference { get; set; }

            public static implicit operator Qualification(GetApplicationViewQueryResult.EducationHistorySection.Qualification source)
            {
                return new Qualification
                {
                    Id = source.Id,
                    Subject = source.Subject,
                    Grade = source.Grade,
                    AdditionalInformation = source.AdditionalInformation,
                    IsPredicted = source.IsPredicted,
                    QualificationOrder = source.QualificationOrder,
                    QualificationReference = source.QualificationReference,
                    QualificationReferenceId = source.QualificationReference.Id,
                };
            }
        }

        public record QualificationReference
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public static implicit operator QualificationReference(GetApplicationViewQueryResult.EducationHistorySection.QualificationReference source)
            {
                return new QualificationReference
                {
                    Id = source.Id,
                    Name = source.Name
                };
            }
        }
    }

    public record WorkHistorySection
    {
        public List<Job> Jobs { get; set; } = [];
        public List<VolunteeringAndWorkExperience> VolunteeringAndWorkExperiences { get; set; } = [];

        public static implicit operator WorkHistorySection(GetApplicationViewQueryResult.WorkHistorySection source)
        {
            return new WorkHistorySection
            {
                Jobs = source.Jobs?.Select(x => (Job)x).ToList() ?? [],
                VolunteeringAndWorkExperiences = source.VolunteeringAndWorkExperiences?.Select(x => (VolunteeringAndWorkExperience)x).ToList() ?? []
            };
        }
        public record Job
        {
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }

            public static implicit operator Job(GetApplicationViewQueryResult.WorkHistorySection.Job source)
            {
                return new Job
                {
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
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }

            public static implicit operator VolunteeringAndWorkExperience(GetApplicationViewQueryResult.WorkHistorySection.VolunteeringAndWorkExperience source)
            {
                return new VolunteeringAndWorkExperience
                {
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
        public Question? AdditionalQuestion1 { get; set; }
        public Question? AdditionalQuestion2 { get; set; }

        public record Question
        {
            public string QuestionLabel { get; set; }
            public string Answer { get; set; }

            public static implicit operator Question(GetApplicationViewQueryResult.ApplicationQuestionsSection.Question source)
            {
                if (source == null)
                {
                    return null;
                }
                return new Question
                {
                    Answer = source.Answer,
                    QuestionLabel = source.QuestionLabel,
                };
            }
        }

        public static implicit operator ApplicationQuestionsSection(GetApplicationViewQueryResult.ApplicationQuestionsSection source)
        {
            return new ApplicationQuestionsSection
            {
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
            };
        }
    }

    public record InterviewAdjustmentsSection
    {
        public string InterviewAdjustmentsDescription { get; set; }

        public static implicit operator InterviewAdjustmentsSection(GetApplicationViewQueryResult.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription
            };
        }
    }

    public record DisabilityConfidenceSection
    {
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }

        public static implicit operator DisabilityConfidenceSection(GetApplicationViewQueryResult.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme
            };
        }
    }

    public record WhatIsYourInterestSection
    {
        public string WhatIsYourInterest { get; set; }
        public static implicit operator WhatIsYourInterestSection(GetApplicationViewQueryResult.WhatIsYourInterestSection source)
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
        public string Support { get; set; }

        public static implicit operator AboutYouSection(GetApplicationViewQueryResult.AboutYouSection source)
        {
            return new AboutYouSection
            {
                Support = source.Support,
                SkillsAndStrengths = source.SkillsAndStrengths
            };
        }
    }
}