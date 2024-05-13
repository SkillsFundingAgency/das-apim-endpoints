﻿using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class GetApplicationApiResponse
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string VacancyReference { get; set; }
        public string QualificationsStatus { get; set; }
        public string TrainingCoursesStatus { get; set; }
        public string JobsStatus { get; set; }
        public string WorkExperienceStatus { get; set; }
        public string SkillsAndStrengthStatus { get; set; }
        public string InterestsStatus { get; set; }
        public string AdditionalQuestion1Status { get; set; }
        public string AdditionalQuestion2Status { get; set; }
        public string InterviewAdjustmentsStatus { get; set; }
        public string DisabilityConfidenceStatus { get; set; }
        public List<Question> AdditionalQuestions { get; set; } = [];
        public string WhatIsYourInterest { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public string ApplicationAllSectionStatus { get; set; }
        public string Status { get; set; }
        public List<GetTrainingCourseApiResponse> TrainingCourses { get; set; } = [];
        public List<Qualification> Qualifications { get; set; } = [];
        public List<GetWorkHistoryItemApiResponse> WorkHistory { get; set; } = [];
        public AboutYouItem AboutYou { get; set; }
        public GetCandidateApiResponse Candidate { get; set; }
        public DateTime SubmittedDate { get; set; }
    }

    public class Question
    {
        public required Guid Id { get; init; }
        public required string QuestionText { get; init; }
        public string Answer { get; set; }
    }
}
