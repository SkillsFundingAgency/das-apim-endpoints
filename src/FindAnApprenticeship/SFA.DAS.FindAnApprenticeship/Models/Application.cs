using System;

namespace SFA.DAS.FindAnApprenticeship.Models
{
    public class Application
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string? DisabilityStatus { get; set; }
        public string VacancyReference { get; set; }
        public ApplicationStatus Status { get; set; }
        public SectionStatus TrainingCoursesStatus { get; set; }
        public SectionStatus WorkExperienceStatus { get; set; }
        public SectionStatus QualificationsStatus { get; set; }
        public SectionStatus JobsStatus { get; set; }
        public SectionStatus DisabilityConfidenceStatus { get; set; }
        public SectionStatus SkillsAndStrengthStatus { get; set; }
        public SectionStatus InterviewAdjustmentsStatus { get; set; }
        public SectionStatus AdditionalQuestion2Status { get; set; }
        public SectionStatus AdditionalQuestion1Status { get; set; }
        public SectionStatus InterestsStatus { get; set; }
        public SectionStatus EducationHistorySectionStatus { get; set; }
        public SectionStatus WorkHistorySectionStatus { get; set; }
        public SectionStatus ApplicationQuestionsSectionStatus { get; set; }
        public SectionStatus InterviewAdjustmentsSectionStatus { get; set; }
        public SectionStatus DisabilityConfidenceSectionStatus { get; set; }
        public string WhatIsYourInterest { get; set; }
    }
}