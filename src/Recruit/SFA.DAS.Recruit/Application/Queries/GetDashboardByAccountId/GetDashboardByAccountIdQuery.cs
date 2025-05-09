using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId
{
    public record GetDashboardByAccountIdQuery(long AccountId, ApplicationStatus Status)
        : IRequest<GetDashboardByAccountIdQueryResult>;
}