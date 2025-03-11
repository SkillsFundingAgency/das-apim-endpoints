using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;

public class GetAccountFinanceQueryResult
{
    public IEnumerable<PayeScheme> PayeSchemes { get; set; } = [];
    public IEnumerable<Transaction> Transactions { get; set; } = [];
    public decimal Balance { get; set; }

}
