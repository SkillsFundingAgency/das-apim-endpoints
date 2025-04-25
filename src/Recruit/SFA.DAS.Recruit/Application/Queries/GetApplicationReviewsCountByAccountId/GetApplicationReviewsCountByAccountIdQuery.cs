using MediatR;
using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByAccountId
{
    public record GetApplicationReviewsCountByAccountIdQuery(long AccountId, List<long> VacancyReferences)
        : IRequest<List<ApplicationReviewStats>>;
}