using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.SharedOuterApi.Domain.Recruit.Ai;

namespace SFA.DAS.RecruitJobs.UnitTests.Ai;

public class WhenFlaggingForReview
{
    [Test, MoqAutoData]
    public void Then_A_Passing_Review_Will_Not_Get_Flagged(
        Mock<AiReviewResultV1> aiResult,
        [Frozen] Mock<IRandomNumberGenerator> generator,
        [Greedy] AiReviewResultChecker sut)
    {
        // arrange
        aiResult.Setup(x => x.GetScore()).Returns(0);
        generator.Setup(x => x.NextDouble()).Returns(0);

        // act
        var actual = sut.FlagForReview(aiResult.Object, out var status);

        // assert
        actual.Should().BeFalse();
        status.Should().Be(AiReviewStatus.Passed);
    }
    
    [Test]
    [MoqInlineAutoData(0.94, false)]
    [MoqInlineAutoData(0.95, true)]
    public void Then_A_Passing_Review_Has_A_5_Percent_Chance_To_Be_Flagged_For_Review(
        double chance,
        bool isFlagged,
        Mock<AiReviewResultV1> aiResult,
        [Frozen] Mock<IRandomNumberGenerator> generator,
        [Greedy] AiReviewResultChecker sut)
    {
        // arrange
        aiResult.Setup(x => x.GetScore()).Returns(0);
        generator.Setup(x => x.NextDouble()).Returns(chance);

        // act
        var actual = sut.FlagForReview(aiResult.Object, out var status);

        // assert
        actual.Should().Be(isFlagged);
        status.Should().Be(AiReviewStatus.Passed);
    }
    
    [Test]
    [MoqInlineAutoData(0)]
    [MoqInlineAutoData(0.5)]
    [MoqInlineAutoData(0.95)]
    public void Then_A_Failed_Review_Will_Always_Get_Flagged(
        double chance,
        Mock<AiReviewResultV1> aiResult,
        [Frozen] Mock<IRandomNumberGenerator> generator,
        [Greedy] AiReviewResultChecker sut)
    {
        // arrange
        aiResult.Setup(x => x.GetScore()).Returns(1);
        generator.Setup(x => x.NextDouble()).Returns(chance);

        // act
        var actual = sut.FlagForReview(aiResult.Object, out var status);

        // assert
        actual.Should().BeTrue();
        status.Should().Be(AiReviewStatus.Failed);
    }
    
    [Test]
    [MoqInlineAutoData(0.99, 0, false)]
    [MoqInlineAutoData(0.99, 0.01, true)]
    [MoqInlineAutoData(0.5, 0.49, false)]
    [MoqInlineAutoData(0.5, 0.5, true)]
    [MoqInlineAutoData(0.2, 0, false)]
    [MoqInlineAutoData(0.2, 0.8, true)]
    public void Then_A_Partial_Failure_Has_A_Chance_Equal_To_Its_Score_To_Get_Flagged(
        double score,
        double chance,
        bool isFlagged,
        Mock<AiReviewResultV1> aiResult,
        [Frozen] Mock<IRandomNumberGenerator> generator,
        [Greedy] AiReviewResultChecker sut)
    {
        // arrange
        aiResult.Setup(x => x.GetScore()).Returns(score);
        generator.Setup(x => x.NextDouble()).Returns(chance);

        // act
        var actual = sut.FlagForReview(aiResult.Object, out var status);

        // assert
        actual.Should().Be(isFlagged);
        status.Should().Be(AiReviewStatus.Passed);
    }
}