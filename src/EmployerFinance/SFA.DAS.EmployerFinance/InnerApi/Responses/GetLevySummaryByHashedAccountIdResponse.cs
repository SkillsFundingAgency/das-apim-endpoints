namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public record GetLevySummaryByHashedAccountIdResponse
{
    public decimal CurrentLevyFunds { get; set; }
}