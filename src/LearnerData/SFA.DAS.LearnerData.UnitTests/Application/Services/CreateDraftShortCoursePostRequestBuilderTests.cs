using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class CreateDraftShortCoursePostRequestBuilderTests
{
    private Fixture _fixture;
    private CreateDraftShortCoursePostRequestBuilder _sut;
    private Mock<IShortCourseLookupService> _shortCourseLookupService;

    private const int ExpectedPrice = 2500;
    private const LearningType ExpectedLearningType = LearningType.ApprenticeshipUnit;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _shortCourseLookupService = new Mock<IShortCourseLookupService>();

        _shortCourseLookupService
            .Setup(x => x.GetCourseDetails(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new ShortCourseLookupResult { Price = ExpectedPrice, LearningType = ExpectedLearningType });

        _sut = new CreateDraftShortCoursePostRequestBuilder(
            Mock.Of<ILogger<CreateDraftShortCoursePostRequestBuilder>>(),
            _shortCourseLookupService.Object);
    }

    [Test]
    public async Task Build_Should_Map_AllFields_Correctly()
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
        var result = await _sut.Build(request, ukprn);

        // Assert
        result.LearnerUpdateDetails.Uln.Should().Be(learner.Uln);
        result.LearnerUpdateDetails.FirstName.Should().Be(learner.FirstName);
        result.LearnerUpdateDetails.LastName.Should().Be(learner.LastName);
        result.LearnerUpdateDetails.DateOfBirth.Should().Be(learner.Dob);
        result.LearnerUpdateDetails.EmailAddress.Should().Be(learner.Email);
        result.LearnerUpdateDetails.LearnerRef.Should().Be(learner.LearnerRef);

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
        result.OnProgramme.Price.Should().Be(ExpectedPrice);
        result.OnProgramme.LearningType.Should().Be(ExpectedLearningType);

        result.OnProgramme.Milestones.Should().BeEquivalentTo(onProgramme.Milestones);
    }

    [Test]
    public async Task Build_Calls_LookupService_With_CourseCode_And_StartDate()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var onProgramme = _fixture.Build<ShortCourseOnProgramme>()
            .With(x => x.CourseCode, "ZSC00001")
            .With(x => x.StartDate, new DateTime(2026, 8, 1))
            .With(x => x.Milestones, Array.Empty<Milestone>())
            .With(x => x.LearningSupport, new List<LearningSupportRequestDetails>())
            .With(x => x.CompletionDate, (DateTime?)null)
            .Create();

        var request = _fixture.Build<ShortCourseRequest>()
            .With(x => x.Delivery, new ShortCourseDelivery { OnProgramme = [onProgramme] })
            .Create();

        // Act
        await _sut.Build(request, ukprn);

        // Assert
        _shortCourseLookupService.Verify(x => x.GetCourseDetails("ZSC00001", new DateTime(2026, 8, 1)), Times.Once);
    }

    [Test]
    public async Task Build_Adds_LearningComplete_Milestone_When_CompletionDate_Set_And_Milestone_Absent()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var onProgramme = _fixture.Build<ShortCourseOnProgramme>()
            .With(x => x.CompletionDate, DateTime.UtcNow.AddMonths(5))
            .With(x => x.Milestones, new[] { Milestone.ThirtyPercentLearningComplete })
            .With(x => x.LearningSupport, new List<LearningSupportRequestDetails>())
            .Create();

        var request = _fixture.Build<ShortCourseRequest>()
            .With(x => x.Delivery, new ShortCourseDelivery { OnProgramme = [onProgramme] })
            .Create();

        // Act
        var result = await _sut.Build(request, ukprn);

        // Assert
        result.OnProgramme.Milestones.Should().Contain(SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone.LearningComplete);
    }

    [Test]
    public async Task Build_Does_Not_Duplicate_LearningComplete_When_Already_Present()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var onProgramme = _fixture.Build<ShortCourseOnProgramme>()
            .With(x => x.CompletionDate, DateTime.UtcNow.AddMonths(5))
            .With(x => x.Milestones, new[] { Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete })
            .With(x => x.LearningSupport, new List<LearningSupportRequestDetails>())
            .Create();

        var request = _fixture.Build<ShortCourseRequest>()
            .With(x => x.Delivery, new ShortCourseDelivery { OnProgramme = [onProgramme] })
            .Create();

        // Act
        var result = await _sut.Build(request, ukprn);

        // Assert
        result.OnProgramme.Milestones.Should().ContainSingle(m =>
            m == SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone.LearningComplete);
    }

    [Test]
    public async Task Build_Does_Not_Add_LearningComplete_When_CompletionDate_Not_Set()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var onProgramme = _fixture.Build<ShortCourseOnProgramme>()
            .With(x => x.CompletionDate, (DateTime?)null)
            .With(x => x.Milestones, new[] { Milestone.ThirtyPercentLearningComplete })
            .With(x => x.LearningSupport, new List<LearningSupportRequestDetails>())
            .Create();

        var request = _fixture.Build<ShortCourseRequest>()
            .With(x => x.Delivery, new ShortCourseDelivery { OnProgramme = [onProgramme] })
            .Create();

        // Act
        var result = await _sut.Build(request, ukprn);

        // Assert
        result.OnProgramme.Milestones.Should().NotContain(SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses.Milestone.LearningComplete);
    }
}
