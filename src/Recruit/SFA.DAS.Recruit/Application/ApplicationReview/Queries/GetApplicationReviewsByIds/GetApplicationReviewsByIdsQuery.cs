using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByIds;

public record GetApplicationReviewsByIdsQuery(List<Guid> ApplicationIds)
    : IRequest<GetApplicationReviewsByIdsQueryResult>;
