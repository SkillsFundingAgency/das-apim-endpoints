namespace SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;

public sealed record GetLatestTransactionTypeTransactionIdsForAccountResponse
{
    public long? LatestDeclarationId { get; init; }
    public long? LatestPaymentId { get; init; }
    public long? LatestExpiryId { get; init; }
}
