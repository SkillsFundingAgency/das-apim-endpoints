namespace SFA.DAS.RecruitJobs.Domain;

public enum ReviewStatus : byte
{
    New,
    PendingReview,
    UnderReview,
    Closed
}