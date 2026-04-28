using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.RecruitJobs.InnerApi.Responses.ApplicationReviews;

public record GetApplicationReviewApiResponse
{
    public Guid Id { get; init; }
    public long VacancyReference { get; init; }
    public ApplicationReviewStatus Status { get; init; }
}