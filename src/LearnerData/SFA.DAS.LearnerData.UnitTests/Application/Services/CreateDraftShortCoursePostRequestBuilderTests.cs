using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class CreateDraftShortCoursePostRequestBuilderTests
{
    private Fixture _fixture;
    private CreateDraftShortCoursePostRequestBuilder _sut;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _sut = new CreateDraftShortCoursePostRequestBuilder();
    }

    [Test]
    public void Build_Should_Map_AllFields_Correctly()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learner = _fixture.Build<ShortCourseLearnerRequestDetails>()
            .With(x => x.FirstName, "Frank")
            .With(x => x.LastName, "Frankinson")
            .With(x => x.Dob, new DateTime(2000, 1, 1))
            .With(x => x.Email, "frank@example.com")
            .Create();

        var onProgramme = _fixture.Build<ShortCourseOnProgramme>()
            .With(x => x.CourseCode, "123")
            .With(x => x.AgreementId, "789")
            .With(x => x.StartDate, DateTime.UtcNow)
            .With(x => x.ExpectedEndDate, DateTime.UtcNow.AddMonths(6))
            .With(x => x.CompletionDate, DateTime.UtcNow.AddMonths(5))
            .With(x => x.WithdrawalDate, (DateTime?)null)
            .With(x => x.LearningSupport, _fixture.CreateMany<LearningSupportRequestDetails>(2).ToList())
            .With(x => x.Milestones, new[] { Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete })
            .Create();

        var request = _fixture.Build<ShortCourseRequest>()
            .With(x => x.Learner, learner)
            .With(x => x.Delivery, new ShortCourseDelivery { OnProgramme = [onProgramme] })
            .Create();

        // Act
        var result = _sut.Build(request, ukprn);

        // Assert
        result.LearnerUpdateDetails.Uln.Should().Be(learner.Uln);
        result.LearnerUpdateDetails.FirstName.Should().Be(learner.FirstName);
        result.LearnerUpdateDetails.LastName.Should().Be(learner.LastName);
        result.LearnerUpdateDetails.DateOfBirth.Should().Be(learner.Dob);
        result.LearnerUpdateDetails.EmailAddress.Should().Be(learner.Email);

        result.LearningSupport.Should().HaveCount(onProgramme.LearningSupport.Count);
        for (int i = 0; i < onProgramme.LearningSupport.Count; i++)
        {
            result.LearningSupport[i].StartDate.Should().Be(onProgramme.LearningSupport[i].StartDate);
            result.LearningSupport[i].EndDate.Should().Be(onProgramme.LearningSupport[i].EndDate);
        }

        result.OnProgramme.CourseCode.Should().Be(onProgramme.CourseCode);
        result.OnProgramme.Ukprn.Should().Be(ukprn);
        result.OnProgramme.StartDate.Should().Be(onProgramme.StartDate);
        result.OnProgramme.ExpectedEndDate.Should().Be(onProgramme.ExpectedEndDate);
        result.OnProgramme.CompletionDate.Should().Be(onProgramme.CompletionDate);
        result.OnProgramme.WithdrawalDate.Should().Be(onProgramme.WithdrawalDate);
        result.OnProgramme.Price.Should().Be(1000m);

        result.OnProgramme.Milestones.Should().BeEquivalentTo(onProgramme.Milestones);
    }
}