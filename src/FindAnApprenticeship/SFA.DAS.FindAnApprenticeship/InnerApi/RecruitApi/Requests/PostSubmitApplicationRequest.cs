using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;

public class PostSubmitApplicationRequest(Guid candidateId, PostSubmitApplicationRequestData data) : IPostApiRequest
{
    public string PostUrl => $"api/applications/{candidateId}";
    public object Data { get; set; } = data;

    
}

public class PostSubmitApplicationRequestData
{
    public static implicit operator PostSubmitApplicationRequestData(GetApplicationApiResponse source)
    {
        return new PostSubmitApplicationRequestData
        {
            ApplicationId = source.Id,
            CandidateId = source.CandidateId,
            VacancyReference = source.VacancyReference,
            Jobs = source.WorkHistory?.Where(c=>c.WorkHistoryType == WorkHistoryType.Job)
                .Select(c=>new WorkHistory
                {
                    JobTitle = c.JobTitle,
                    Employer = c.Employer,
                    ToDate = c.EndDate,
                    Description = c.Description,
                    FromDate = c.StartDate
                }).ToList(),
            WorkExperiences = source.WorkHistory?.Where(c=>c.WorkHistoryType == WorkHistoryType.WorkExperience)
                .Select(c=>new WorkHistory
                {
                    JobTitle = c.JobTitle,
                    Employer = c.Employer,
                    ToDate = c.EndDate,
                    Description = c.Description,
                    FromDate = c.StartDate
                }).ToList(),
            Qualifications = source.Qualifications?.Select(c=> new ApplicationQualification
            {
                Subject = c.Subject,
                Grade = c.Grade,
                IsPredicted = c.IsPredicted,
                QualificationType = c.QualificationReference.Name,
                AdditionalInformation = c.AdditionalInformation
            }).ToList(),
            TrainingCourses = source.TrainingCourses?.Select(c=>new TrainingCourse
            {
                Title = c.CourseName,
                ToDate = new DateTime(c.YearAchieved,1,1)
            }).ToList(),
            FirstName = source.Candidate.FirstName,
            LastName = source.Candidate.LastName,
            Email = source.Candidate.Email,
            Phone = source.Candidate.PhoneNumber,
            AddressLine1 = source.Candidate.Address.AddressLine1,
            AddressLine2 = source.Candidate.Address.AddressLine2,
            AddressLine3 = source.Candidate.Address.Town,
            AddressLine4 = source.Candidate.Address.County,
            Postcode = source.Candidate.Address.Postcode,
            HobbiesAndInterests = source.AboutYou.HobbiesAndInterests,
            Strengths = source.AboutYou.SkillsAndStrengths,
            Improvements = source.AboutYou.Improvements,
            ApplicationDate = DateTime.UtcNow,
            AdditionalQuestion1 = source.AdditionalQuestions?.FirstOrDefault() != null ? new AdditionalQuestion
            {
                AnswerText = source.AdditionalQuestions.FirstOrDefault().Answer,
                QuestionText = source.AdditionalQuestions.FirstOrDefault().QuestionText,
            } : null,
            AdditionalQuestion2 = source.AdditionalQuestions?.LastOrDefault() != null ? new AdditionalQuestion
            {
                AnswerText = source.AdditionalQuestions.LastOrDefault().Answer,
                QuestionText = source.AdditionalQuestions.LastOrDefault().QuestionText,
            } : null,
            Support = source.AboutYou.Support,
            DisabilityConfidenceStatus = source.DisabilityConfidenceStatus
        };
    }
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public long VacancyReference { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Postcode { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string DisabilityConfidenceStatus { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string HobbiesAndInterests { get; set; }
    public string Improvements { get; set; }
    
    public string Phone { get; set; }
    
    public List<ApplicationQualification> Qualifications { get; set; }
    public string Strengths { get; set; }
    public string Support { get; set; }
    public List<TrainingCourse> TrainingCourses { get; set; }
    public List<WorkHistory> WorkExperiences { get; set; }
    public List<WorkHistory> Jobs { get; set; }
    public AdditionalQuestion? AdditionalQuestion1 { get; set; }
    public AdditionalQuestion? AdditionalQuestion2 { get; set; }
    
}
public class ApplicationQualification
{
    public string QualificationType { get; set; }
    public string Subject { get; set; }
    public string Grade { get; set; }
    public bool? IsPredicted { get; set; }
    public string AdditionalInformation { get; set; }
}

public class TrainingCourse
{
    public string Title { get; set; }
    public DateTime ToDate { get; set; }
}
public class WorkHistory
{
    public string Employer { get; set; }
    public string JobTitle { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class AdditionalQuestion
{
    public string QuestionText { get; set; }
    public string AnswerText { get; set; }
}