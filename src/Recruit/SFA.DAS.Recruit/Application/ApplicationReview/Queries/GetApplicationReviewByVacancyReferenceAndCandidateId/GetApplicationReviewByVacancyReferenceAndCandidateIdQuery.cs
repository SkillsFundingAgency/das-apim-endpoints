using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewByVacancyReferenceAndCandidateId;

public record GetApplicationReviewByVacancyReferenceAndCandidateIdQuery(long VacancyReference, Guid CandidateId)
    : IRequest<GetApplicationReviewByVacancyReferenceAndCandidateIdQueryResult>;
