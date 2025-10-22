using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByIds;
public record GetApplicationReviewsByIdsQueryResult
{
    public List<Domain.ApplicationReview> ApplicationReviews { get; init; } = [];
}