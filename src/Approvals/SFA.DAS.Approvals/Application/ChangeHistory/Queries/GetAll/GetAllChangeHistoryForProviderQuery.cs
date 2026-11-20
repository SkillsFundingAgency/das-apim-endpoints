using MediatR;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

public class GetAllChangeHistoryForProviderQuery(long providerId) : IRequest<GetAllChangeHistoryForProviderQueryResult>
{
    public long ProviderId { get; } = providerId;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}