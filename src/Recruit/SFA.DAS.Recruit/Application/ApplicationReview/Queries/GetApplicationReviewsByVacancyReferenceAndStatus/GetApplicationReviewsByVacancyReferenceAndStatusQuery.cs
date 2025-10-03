using MediatR;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndStatus;

public record GetApplicationReviewsByVacancyReferenceAndStatusQuery(
    long VacancyReference,
    ApplicationReviewStatus Status,
    bool IncludeTemporaryStatus) : IRequest<GetApplicationReviewsByVacancyReferenceAndStatusQueryResult>;