using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class PutApplicationApiResponse
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }

        public string VacancyReference { get; set; }
        public string Status { get; set; }

        public string QualificationStatus { get; set; }
        public string TrainingCourseStatus { get; set; }

        public string JobStatus { get; set; }
        public string WorkExperienceStatus { get; set; }

        public string SkillsAndStrengthsStatus { get; set; }
        public string InterestsStatus { get; set; }
        public string AdditionalQuestion1Status { get; set; }
        public string AdditionalQuestion2Status { get; set; }

        public string InterviewAdjustmentsStatus { get; set; }
        public string DisabilityConfidenceStatus { get; set; }
    }
}
