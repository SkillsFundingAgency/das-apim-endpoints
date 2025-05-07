using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByAccountId
{
    public record GetApplicationReviewsCountByAccountIdQuery(long AccountId, List<long> VacancyReferences)
        : IRequest<GetApplicationReviewsCountByAccountIdQueryResult>;
}