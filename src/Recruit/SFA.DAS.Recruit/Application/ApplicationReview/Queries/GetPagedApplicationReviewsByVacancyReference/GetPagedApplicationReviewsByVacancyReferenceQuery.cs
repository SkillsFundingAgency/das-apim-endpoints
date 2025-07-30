using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetPagedApplicationReviewsByVacancyReference;

public sealed record GetPagedApplicationReviewsByVacancyReferenceQuery(
    long VacancyReference,
    int PageNumber = 1,
    int PageSize = 10,
    string SortColumn = "CreatedDate",
    bool IsAscending = false) : IRequest<GetPagedApplicationReviewsByVacancyReferenceQueryResult>;