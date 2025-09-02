using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference
{
    public sealed record GetApplicationReviewsByVacancyReferenceQuery(
        long VacancyReference)
        : IRequest<GetApplicationReviewsByVacancyReferenceQueryResult>;
}