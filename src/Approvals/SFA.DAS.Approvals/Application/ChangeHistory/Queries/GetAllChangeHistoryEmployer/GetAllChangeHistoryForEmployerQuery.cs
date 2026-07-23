using MediatR;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

public class GetAllChangeHistoryForEmployerQuery(long accountId) : IRequest<GetAllChangeHistoryForEmployerQueryResult>
{
    public long AccountId { get; } = accountId;
}