using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using SFA.DAS.SharedOuterApi.Domain;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplication
{
    public record GetApplicationViewQueryResult
    {
        public AboutYouSection AboutYou { get; set; }
        public ApplicationQuestionsSection ApplicationQuestions { get; set; }
        public ApprenticeshipTypes? ApprenticeshipType { get; set; } = ApprenticeshipTypes.Standard;
        public bool IsDisabilityConfident { get; set; }
        public Candidate CandidateDetails { get; set; }
        public DateTime? MigrationDate { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public DisabilityConfidenceSection DisabilityConfidence { get; set; }
        public EducationHistorySection EducationHistory { get; set; }
        public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
        public string ApplicationStatus { get; set; }
        public VacancyDetailsSection VacancyDetails { get; set; }
        public WhatIsYourInterestSection WhatIsYourInterest { get; set; }
        public WorkHistorySection WorkHistory { get; set; }
        public EmploymentLocationSection? EmploymentLocation { get; set; }

        public record VacancyDetailsSection
        {
            public string Title { get; set; }
            public string EmployerName { get; set; }
        }

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
            public List<TrainingCourse> TrainingCourses { get; set; } = [];
            public List<Qualification> Qualifications { get; set; } = [];
            public List<QualificationReference> QualificationTypes { get; set; } = [];
            public record TrainingCourse
            {
                public string CourseName { get; set; }
                public int YearAchieved { get; set; }

                public static implicit operator TrainingCourse(GetTrainingCourseApiResponse source)
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
                public short? QualificationOrder { get; set; }
                public QualificationReference QualificationReference { get; set; }

                public static implicit operator Qualification(InnerApi.CandidateApi.Responses.Qualification source)
                {
                    return new Qualification
                    {
                        Id = source.Id,
                        Subject = source.Subject,
                        Grade = source.Grade,
                        AdditionalInformation = source.AdditionalInformation,
                        IsPredicted = source.IsPredicted,
                        QualificationOrder = source.QualificationOrder,
                        QualificationReference = source.QualificationReference
                    };
                }
            }

            public record QualificationReference
            {
                public Guid Id { get; set; }
                public string Name { get; set; }

                public static implicit operator QualificationReference(InnerApi.CandidateApi.Responses.QualificationReference source)
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
            public record Job
            {
                public string Employer { get; set; }
                public string JobTitle { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime? EndDate { get; set; }
                public string Description { get; set; }

                public static implicit operator Job(GetWorkHistoryItemApiResponse source)
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

                public static implicit operator VolunteeringAndWorkExperience(GetWorkHistoryItemApiResponse source)
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

        public record EmploymentLocationSection : LocationDto
        {

        }

        public record ApplicationQuestionsSection
        {
            public Question? AdditionalQuestion1 { get; set; }
            public Question? AdditionalQuestion2 { get; set; }

            public record Question
            {
                public string QuestionLabel { get; set; }
                public string Answer { get; set; }
            }
        }

        public record InterviewAdjustmentsSection
        {
            public string InterviewAdjustmentsDescription { get; set; }
        }

        public record DisabilityConfidenceSection
        {
            public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        }

        public record WhatIsYourInterestSection
        {
            public string WhatIsYourInterest { get; set; }
        }

        public record AboutYouSection
        {
            public string SkillsAndStrengths { get; set; }
            public string Support { get; set; }
        }
    }
}