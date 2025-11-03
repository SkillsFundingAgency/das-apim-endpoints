using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetAlertsByAccountId;
public record GetAlertsByAccountIdQuery(long AccountId,
    string UserId): IRequest<GetAlertsByAccountIdQueryResult>;