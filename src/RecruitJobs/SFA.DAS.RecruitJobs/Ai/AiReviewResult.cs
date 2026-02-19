namespace SFA.DAS.RecruitJobs.Ai;

public abstract class AiReviewResult
{
    public int Version => 1; // DO NOT REMOVE
    public abstract double GetScore();
}