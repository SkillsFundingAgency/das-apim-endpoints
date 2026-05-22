using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyId;

public sealed record GetApplicationReviewsByVacancyIdQueryResult(List<Domain.ApplicationReview> ApplicationReviews);