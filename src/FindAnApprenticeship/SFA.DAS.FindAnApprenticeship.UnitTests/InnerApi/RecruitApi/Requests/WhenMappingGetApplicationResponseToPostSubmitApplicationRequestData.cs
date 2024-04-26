using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.RecruitApi.Requests;

public class WhenMappingGetApplicationResponseToPostSubmitApplicationRequestData
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetApplicationApiResponse source)
    {
        var actual = (PostSubmitApplicationRequestData)source;

        actual.ApplicationId.Should().Be(source.Id);
        actual.CandidateId.Should().Be(source.CandidateId);
        actual.VacancyReference.Should().Be(source.VacancyReference);
        actual.TrainingCourses.Should().BeEquivalentTo(source.TrainingCourses.Select(c => new TrainingCourse
        {
            ToDate = new DateTime(c.YearAchieved,1,1),
            Title = c.CourseName
        }).ToList());
        actual.Jobs.Should().BeEquivalentTo(source.WorkHistory.Where(x => x.WorkHistoryType == WorkHistoryType.Job)
            .Select(c => new WorkHistory
            {
                JobTitle = c.JobTitle,
                Employer = c.Employer,
                ToDate = c.EndDate,
                Description = c.Description,
                FromDate = c.StartDate
            }).ToList());
        actual.WorkExperiences.Should().BeEquivalentTo(source.WorkHistory.Where(x => x.WorkHistoryType == WorkHistoryType.WorkExperience)
            .Select(c => new WorkHistory
            {
                JobTitle = c.JobTitle,
                Employer = c.Employer,
                ToDate = c.EndDate,
                Description = c.Description,
                FromDate = c.StartDate
            }).ToList());
        actual.Qualifications.Should().BeEquivalentTo(source.Qualifications.Select(c => new ApplicationQualification
        {
            Subject = c.Subject,
            Grade = c.Grade,
            IsPredicted = c.IsPredicted,
            QualificationType = c.QualificationReference.Name,
            AdditionalInformation = c.AdditionalInformation
        }).ToList());
        actual.FirstName.Should().Be(source.Candidate.FirstName);
        actual.LastName.Should().Be(source.Candidate.LastName);
        actual.AddressLine1.Should().Be(source.Candidate.Address.AddressLine1);
        actual.AddressLine2.Should().Be(source.Candidate.Address.AddressLine2);
        actual.AddressLine3.Should().Be(source.Candidate.Address.Town);
        actual.AddressLine4.Should().Be(source.Candidate.Address.County);
        actual.Postcode.Should().Be(source.Candidate.Address.Postcode);
        actual.Email.Should().Be(source.Candidate.Email);
        actual.Phone.Should().Be(source.Candidate.PhoneNumber);
        actual.HobbiesAndInterests.Should().Be(source.AboutYou.HobbiesAndInterests);
        actual.Strengths.Should().Be(source.AboutYou.SkillsAndStrengths);
        actual.Improvements.Should().Be(source.AboutYou.Improvements);
        actual.ApplicationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        actual.AdditionalQuestion1.Should().BeEquivalentTo(new AdditionalQuestion{AnswerText = source.AdditionalQuestions.FirstOrDefault().Answer, QuestionText = source.AdditionalQuestions.FirstOrDefault().QuestionText});
        actual.AdditionalQuestion2.Should().BeEquivalentTo(new AdditionalQuestion{AnswerText = source.AdditionalQuestions.LastOrDefault().Answer, QuestionText = source.AdditionalQuestions.LastOrDefault().QuestionText});
        actual.Support.Should().Be(source.AboutYou.Support);
        actual.DisabilityConfidenceStatus.Should().Be(source.DisabilityConfidenceStatus);
    }

    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped_When_There_Are_Optional(GetApplicationApiResponse source)
    {
        source.AdditionalQuestions = null;
        source.Qualifications = null;
        source.WorkHistory = null;
        source.TrainingCourses = null; 
        source.DisabilityConfidenceStatus = null;
        
        var actual = (PostSubmitApplicationRequestData)source;

        actual.AdditionalQuestion1.Should().BeNull();
        actual.AdditionalQuestion2.Should().BeNull();
        actual.Qualifications.Should().BeNull();
        actual.TrainingCourses.Should().BeNull();
        actual.Jobs.Should().BeNull();
        actual.WorkExperiences.Should().BeNull();
    }
}