using System;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyId;

public sealed record GetApplicationReviewsByVacancyIdQuery(Guid VacancyId)
    : MediatR.IRequest<GetApplicationReviewsByVacancyIdQueryResult>;