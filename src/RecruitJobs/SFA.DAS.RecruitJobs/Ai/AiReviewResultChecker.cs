using SFA.DAS.SharedOuterApi.Domain.Recruit.Ai;

namespace SFA.DAS.RecruitJobs.Ai;

public interface IAiReviewResultChecker
{
    bool FlagForReview(AiReviewResult review, out AiReviewStatus status);
}

public class AiReviewResultChecker(IRandomNumberGenerator generator): IAiReviewResultChecker
{
    public bool FlagForReview(AiReviewResult review, out AiReviewStatus status)
    {
        status = AiReviewStatus.Failed;
        var totalScore = review.GetScore();

        switch (totalScore)
        {
            case 0: // 5% chance of a review with no problems getting reviewed
            {
                status = AiReviewStatus.Passed;
                return 0.05 + generator.NextDouble() >= 1;
            }
            case < 1: 
            {
                status = AiReviewStatus.Passed;
                return totalScore + generator.NextDouble() >= 1;
            }
            default: return true;
        }
    }
}