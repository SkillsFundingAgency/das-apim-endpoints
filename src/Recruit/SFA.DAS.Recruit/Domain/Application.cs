#nullable enable
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Models;
using JsonException = System.Text.Json.JsonException;

namespace SFA.DAS.Recruit.Domain
{
    public record Application
    {
        public ApplicationStatus Status { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public Candidate? Candidate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? MigrationDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public Guid CandidateId { get; set; }
        public Guid Id { get; set; }
        public Guid? PreviousAnswersSourceId { get; set; }
        public List<AdditionalQuestion>? AdditionalQuestions { get; set; } = [];
        public List<Qualification> Qualifications { get; set; } = [];
        public List<TrainingCourseItem> TrainingCourses { get; set; } = [];
        public List<WorkHistoryItem> WorkHistory { get; set; } = [];
        public Location? EmploymentLocation { get; set; }
        public string? DisabilityStatus { get; set; }
        public string? ResponseNotes { get; set; }
        public string? Strengths { get; set; }
        public string? Support { get; set; }
        public string? VacancyReference { get; set; }
        public string? WhatIsYourInterest { get; set; }
    }
    public record Location
    {
        public List<EmploymentAddress>? Addresses { get; set; } = [];
    }

    public record EmploymentAddress
    {
        public bool IsSelected { get; init; }
        public short AddressOrder { get; init; }
        public string? FullAddress { get; init; }

        public Address? Address
        {
            get
            {
                try
                {
                    return FullAddress != null ? JsonConvert.DeserializeObject<Address>(FullAddress) : null;
                }
                catch (JsonException)
                {
                    return null;
                }
            }
        }
    }

    public record AdditionalQuestion
    {
        public Guid Id { get; init; }
        public Guid CandidateId { get; init; }
        public Guid ApplicationId { get; init; }
        public string? QuestionText { get; init; }
        public string? Answer { get; init; }
        public short? QuestionOrder { get; init; }
    }

    public record Qualification
    {
        public Guid Id { get; set; }
        public string? Subject { get; set; }
        public string? Grade { get; set; }
        public string? AdditionalInformation { get; set; }
        public int? ToYear { get; set; }
        public bool? IsPredicted { get; set; }
        public short? QualificationOrder { get; set; }
        public QualificationReference QualificationReference { get; set; } = new();
        public DateTime? CreatedDate { get; set; }
    }

    public record QualificationReference
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    public record WorkHistoryItem
    {
        public Guid Id { get; set; }
        public WorkHistoryType WorkHistoryType { get; set; }
        public string? Employer { get; set; }
        public string? JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string? Description { get; set; }
    }

    public record TrainingCourseItem
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string? CourseName { get; set; }
        public int YearAchieved { get; set; }
    }
}