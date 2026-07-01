using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Services.ShortCourses;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class UpdateShortCourseOnProgrammeEarningPutRequestBuilderTests
{
    private UpdateShortCourseOnProgrammeEarningPutRequestBuilder _sut;

    [SetUp]
    public void SetUp() => _sut = new UpdateShortCourseOnProgrammeEarningPutRequestBuilder();

    [Test]
    public void Build_FromOnProgramme_Maps_Fields()
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";
        var onProgramme = new OnProgramme
        {
            WithdrawalDate = new DateTime(2025, 6, 1),
            CompletionDate = new DateTime(2025, 12, 1),
            StartDate = new DateTime(2025, 1, 1),
            ExpectedEndDate = new DateTime(2025, 6, 30),
            Milestones = [Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete]
        };

        var result = _sut.Build(onProgramme, learnerKey, learnerRef);

        result.WithdrawalDate.Should().Be(onProgramme.WithdrawalDate);
        result.CompletionDate.Should().Be(onProgramme.CompletionDate);
        result.StartDate.Should().Be(onProgramme.StartDate);
        result.ExpectedEndDate.Should().Be(onProgramme.ExpectedEndDate);
        result.Milestones.Should().BeEquivalentTo(onProgramme.Milestones);
        result.LearnerKey.Should().Be(learnerKey);
        result.LearnerRef.Should().Be(learnerRef);
    }

    [Test]
    public void Build_FromShortCourseOnProgramme_Maps_Fields()
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";
        var onProgramme = new ShortCourseOnProgramme
        {
            WithdrawalDate = new DateTime(2025, 6, 1),
            CompletionDate = new DateTime(2025, 12, 1),
            StartDate = new DateTime(2025, 1, 1),
            ExpectedEndDate = new DateTime(2025, 6, 30),
            Milestones = [Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete]
        };

        var result = _sut.Build(onProgramme, learnerKey, learnerRef);

        result.WithdrawalDate.Should().Be(onProgramme.WithdrawalDate);
        result.CompletionDate.Should().Be(onProgramme.CompletionDate);
        result.StartDate.Should().Be(onProgramme.StartDate);
        result.ExpectedEndDate.Should().Be(onProgramme.ExpectedEndDate);
        result.Milestones.Should().BeEquivalentTo(onProgramme.Milestones);
        result.LearnerKey.Should().Be(learnerKey);
        result.LearnerRef.Should().Be(learnerRef);
    }

    [TestCase(nameof(OnProgramme))]
    [TestCase(nameof(ShortCourseOnProgramme))]
    public void Build_Adds_LearningComplete_When_CompletionDate_Set_And_Milestone_Absent(string overload)
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";

        var result = overload == nameof(OnProgramme)
            ? _sut.Build(new OnProgramme { CompletionDate = DateTime.UtcNow, Milestones = [Milestone.ThirtyPercentLearningComplete] }, learnerKey, learnerRef)
            : _sut.Build(new ShortCourseOnProgramme { CompletionDate = DateTime.UtcNow, Milestones = [Milestone.ThirtyPercentLearningComplete] }, learnerKey, learnerRef);

        result.Milestones.Should().Contain(Milestone.LearningComplete);
    }

    [TestCase(nameof(OnProgramme))]
    [TestCase(nameof(ShortCourseOnProgramme))]
    public void Build_Does_Not_Duplicate_LearningComplete_When_Already_Present(string overload)
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";

        var milestones = new[] { Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete };
        var result = overload == nameof(OnProgramme)
            ? _sut.Build(new OnProgramme { CompletionDate = DateTime.UtcNow, Milestones = milestones.ToList() }, learnerKey, learnerRef)
            : _sut.Build(new ShortCourseOnProgramme { CompletionDate = DateTime.UtcNow, Milestones = milestones }, learnerKey, learnerRef);

        result.Milestones.Should().ContainSingle(m => m == Milestone.LearningComplete);
    }

    [TestCase(nameof(OnProgramme))]
    [TestCase(nameof(ShortCourseOnProgramme))]
    public void Build_Does_Not_Add_LearningComplete_When_CompletionDate_Not_Set(string overload)
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";

        var result = overload == nameof(OnProgramme)
            ? _sut.Build(new OnProgramme { CompletionDate = null, Milestones = [Milestone.ThirtyPercentLearningComplete] }, learnerKey, learnerRef)
            : _sut.Build(new ShortCourseOnProgramme { CompletionDate = null, Milestones = [Milestone.ThirtyPercentLearningComplete] }, learnerKey, learnerRef);

        result.Milestones.Should().NotContain(Milestone.LearningComplete);
    }
}
