namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class TransactionsViewModel : List<Transaction>
{
    public int Year { get; set; }
    public int Month { get; set; }
    public bool HasPreviousTransactions { get; set; }
    public string PreviousMonthUri { get; set; } = "";
}