using MediatR;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId
{
    public record GetDashboardByAccountIdQuery(long AccountId, ApplicationReviewStatus Status)
        : IRequest<GetDashboardByAccountIdQueryResult>;
}