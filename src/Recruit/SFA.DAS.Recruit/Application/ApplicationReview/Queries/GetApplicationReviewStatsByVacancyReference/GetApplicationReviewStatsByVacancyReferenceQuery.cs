using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewStatsByVacancyReference;
public record GetApplicationReviewStatsByVacancyReferenceQuery(
    long VacancyReference) : IRequest<GetApplicationReviewStatsByVacancyReferenceQueryResult>;