using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;

public class GetAccountFinanceQuery : IRequest<GetAccountFinanceQueryResult>
{
    public long AccountId { get; set; }
}

