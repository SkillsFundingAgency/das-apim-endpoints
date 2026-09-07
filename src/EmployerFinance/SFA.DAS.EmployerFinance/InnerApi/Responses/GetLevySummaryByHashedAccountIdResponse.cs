namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record GetLevySummaryByHashedAccountIdResponse
{
    public decimal CurrentLevyFunds { get; set; }
    public decimal TotalLevyDeclaredLast12Months { get; set; }
    public decimal TotalLevySpentLast12Months { get; set; }
}