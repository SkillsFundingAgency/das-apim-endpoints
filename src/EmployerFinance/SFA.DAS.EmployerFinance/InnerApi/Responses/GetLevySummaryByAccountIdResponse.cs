namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record GetLevySummaryByAccountIdResponse
{
    public decimal CurrentLevyFunds { get; set; }
    public decimal TotalLevyDeclaredLast12Months { get; set; }
    public decimal TotalLevySpentLast12Months { get; set; }
    public decimal TotalLevyExpiredLast12Months { get; set; }
}