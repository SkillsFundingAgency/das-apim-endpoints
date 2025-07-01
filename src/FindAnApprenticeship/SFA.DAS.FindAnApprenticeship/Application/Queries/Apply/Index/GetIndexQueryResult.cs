﻿using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

public class GetIndexQueryResult
{
    public string VacancyReference { get; set; }
    public string VacancyTitle { get; set; }
    public required ApprenticeshipTypes ApprenticeshipType { get; set; }
    public Address? Address { get; set; }
    public List<Address>? OtherAddresses { get; set; }
    public AvailableWhere? EmployerLocationOption { get; set; }
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

    public class EducationHistorySection
    {
        public string Qualifications { get; set; }
        public string TrainingCourses { get; set; }
    }

    public class WorkHistorySection
    {
        public string Jobs { get; set; }
        public string VolunteeringAndWorkExperience { get; set; }
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
    }

    public class InterviewAdjustmentsSection
    {
        public string RequestAdjustments { get; set; }
    }
    public class DisabilityConfidenceSection
    {
        public string InterviewUnderDisabilityConfident { get; set; }
    }

    public class PreviousApplicationDetails
    {
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}