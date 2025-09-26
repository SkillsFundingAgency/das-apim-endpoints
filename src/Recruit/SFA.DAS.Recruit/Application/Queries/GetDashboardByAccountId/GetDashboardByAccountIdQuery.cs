using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId
{
    public record GetDashboardByAccountIdQuery(long AccountId)
        : IRequest<GetDashboardByAccountIdQueryResult>;
}