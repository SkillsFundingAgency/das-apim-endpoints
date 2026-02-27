using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class CreateUnapprovedShortCourseLearningRequestBuilderTests
{
    private Fixture _fixture;
    private CreateUnapprovedShortCourseLearningRequestBuilder _sut;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _sut = new CreateUnapprovedShortCourseLearningRequestBuilder();
    }

    [Test]
    public void Build_Should_Map_AllFields_Correctly()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningKey = Guid.NewGuid();

        var learner = _fixture.Build<ShortCourseLearnerRequestDetails>()
            .With(x => x.Dob, new DateTime(2000, 1, 1))
            .Create();

        var learningSupport = _fixture.CreateMany<LearningSupportRequestDetails>(2).ToList();

        var startDate = DateTime.UtcNow;
        var expectedEndDate = startDate.AddMonths(6);
        var completionDate = startDate.AddMonths(5);

        var onProgramme = _fixture.Build<ShortCourseOnProgramme>()
            .With(x => x.CourseCode, "123")
            .With(x => x.AgreementId, "789")
            .With(x => x.StartDate, startDate)
            .With(x => x.ExpectedEndDate, expectedEndDate)
            .With(x => x.CompletionDate, completionDate)
            .With(x => x.WithdrawalDate, (DateTime?)null)
            .With(x => x.LearningSupport, learningSupport)
            .With(x => x.Milestones, new[]
            {
                Milestone.ThirtyPercentLearningComplete,
                Milestone.LearningComplete
            })
            .Create();

        var request = _fixture.Build<ShortCourseRequest>()
            .With(x => x.Learner, learner)
            .With(x => x.Delivery, new ShortCourseDelivery { OnProgramme = [onProgramme] })
            .Create();

        // Act
        var result = _sut.Build(request, learningKey, ukprn);

        // Assert
        result.LearningKey.Should().Be(learningKey);

        result.Learner.Uln.Should().Be(learner.Uln.ToString());
        result.Learner.DateOfBirth.Should().Be(learner.Dob);

        result.LearningSupport.Should().HaveCount(learningSupport.Count);
        for (int i = 0; i < learningSupport.Count; i++)
        {
            result.LearningSupport[i].StartDate.Should().Be(learningSupport[i].StartDate);
            result.LearningSupport[i].EndDate.Should().Be(learningSupport[i].EndDate);
        }

        result.OnProgramme.CourseCode.Should().Be(onProgramme.CourseCode);
        result.OnProgramme.EmployerId.Should().Be(long.Parse(onProgramme.AgreementId));
        result.OnProgramme.StartDate.Should().Be(onProgramme.StartDate);
        result.OnProgramme.ExpectedEndDate.Should().Be(onProgramme.ExpectedEndDate);
        result.OnProgramme.CompletionDate.Should().Be(onProgramme.CompletionDate);
        result.OnProgramme.WithdrawalDate.Should().Be(onProgramme.WithdrawalDate);
        result.OnProgramme.Ukprn.Should().Be(ukprn);
        result.OnProgramme.TotalPrice.Should().Be(1000);

        result.OnProgramme.Milestones.Should().BeEquivalentTo(new[]
        {
            SharedOuterApi.InnerApi.Requests.Earnings.Milestone.ThirtyPercentLearningComplete,
            SharedOuterApi.InnerApi.Requests.Earnings.Milestone.LearningComplete
        });
    }
}