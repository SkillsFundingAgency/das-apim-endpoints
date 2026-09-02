namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public record GetLevySummaryByHashedAccountIdResponse
{
    public decimal CurrentLevyFunds { get; set; }
    public decimal TotalLevyDeclaredLast12Months { get; set; }
}