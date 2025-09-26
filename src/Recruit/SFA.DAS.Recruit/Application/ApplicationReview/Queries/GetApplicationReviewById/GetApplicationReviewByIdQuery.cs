using MediatR;
using System;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;

public sealed record GetApplicationReviewByIdQuery(Guid ApplicationReviewId)
    : IRequest<GetApplicationReviewByIdQueryResult>;