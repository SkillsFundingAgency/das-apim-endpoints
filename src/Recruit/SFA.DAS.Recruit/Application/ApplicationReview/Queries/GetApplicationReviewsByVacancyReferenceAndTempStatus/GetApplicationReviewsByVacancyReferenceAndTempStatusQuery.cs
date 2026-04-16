using MediatR;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndTempStatus;

public record GetApplicationReviewsByVacancyReferenceAndTempStatusQuery(
    long VacancyReference,
    ApplicationReviewStatus Status) 
    : IRequest<GetApplicationReviewsByVacancyReferenceAndTempStatusQueryResult>;