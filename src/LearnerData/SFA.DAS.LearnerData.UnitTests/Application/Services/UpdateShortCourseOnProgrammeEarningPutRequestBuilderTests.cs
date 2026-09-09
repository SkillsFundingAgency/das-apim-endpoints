using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Services.ShortCourses;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class UpdateShortCourseOnProgrammeEarningPutRequestBuilderTests
{
    private UpdateShortCourseOnProgrammeEarningPutRequestBuilder _sut;

    [SetUp]
    public void SetUp() => _sut = new UpdateShortCourseOnProgrammeEarningPutRequestBuilder();

    [Test]
    public void Build_Maps_Fields()
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";
        var onProgramme = new ResolvedOnProgramme
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
    public void Build_Adds_LearningComplete_When_CompletionDate_Set_And_Milestone_Absent()
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";

        var result = _sut.Build(new ResolvedOnProgramme { CompletionDate = DateTime.UtcNow, Milestones = [Milestone.ThirtyPercentLearningComplete] }, learnerKey, learnerRef);

        result.Milestones.Should().Contain(Milestone.LearningComplete);
    }

    [Test]
    public void Build_Does_Not_Duplicate_LearningComplete_When_Already_Present()
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";

        var milestones = new[] { Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete };
        var result = _sut.Build(new ResolvedOnProgramme { CompletionDate = DateTime.UtcNow, Milestones = milestones.ToList() }, learnerKey, learnerRef);

        result.Milestones.Should().ContainSingle(m => m == Milestone.LearningComplete);
    }

    [Test]
    public void Build_Does_Not_Add_LearningComplete_When_CompletionDate_Not_Set()
    {
        var learnerKey = Guid.NewGuid();
        var learnerRef = "ABC123";

        var result = _sut.Build(new ResolvedOnProgramme { CompletionDate = null, Milestones = [Milestone.ThirtyPercentLearningComplete] }, learnerKey, learnerRef);

        result.Milestones.Should().NotContain(Milestone.LearningComplete);
    }
}
