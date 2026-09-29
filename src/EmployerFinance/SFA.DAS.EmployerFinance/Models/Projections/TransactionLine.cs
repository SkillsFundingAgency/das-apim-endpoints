using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.EmployerFinance.Models.Enums;

namespace SFA.DAS.EmployerFinance.Models.Projections;

public sealed record TransactionLine
{
    public long AccountId { get; set; }
    public string TransferSourceDescription { get; set; } = null;
    public string Description { get; set; }
    public TransactionItemType TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime DateCreated { get; set; }
    public decimal Amount { get; set; }
    public List<TransactionLine> SubTransactions { get; set; }
    public DateTime PayrollDate { get; set; }
    public string PayrollYear { get; set; }
    public int PayrollMonth { get; set; }
    public decimal Balance { get; set; }
}