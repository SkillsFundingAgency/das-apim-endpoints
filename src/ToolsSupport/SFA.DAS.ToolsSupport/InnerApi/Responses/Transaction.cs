namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class Transaction
{
    public string Description { get; set; } = "";
    public TransactionItemType TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime DateCreated { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public List<Transaction> SubTransactions { get; set; } = [];
    public string ResourceUri { get; set; } = "";
}

public enum TransactionItemType
{
    Unknown = 0,
    Declaration = 1,
    TopUp = 2,
    Payment = 3,
    Transfer = 4,
    ExpiredFund = 5
}
