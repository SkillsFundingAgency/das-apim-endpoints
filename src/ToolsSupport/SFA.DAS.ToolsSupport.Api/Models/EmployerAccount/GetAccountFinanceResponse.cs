using SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;

public class GetAccountFinanceResponse
{
    public IEnumerable<PayeScheme> PayeSchemes { get; set; } = [];
    public IEnumerable<Transaction> Transactions { get; set; } = [];
    public decimal Balance { get; set; }

    public static explicit operator GetAccountFinanceResponse(GetAccountFinanceQueryResult source)
    {
        if (source == null) return new GetAccountFinanceResponse();

        return new GetAccountFinanceResponse
        {
            PayeSchemes = source.PayeSchemes,
            Transactions = source.Transactions,
            Balance = source.Balance
        };
    }
}
