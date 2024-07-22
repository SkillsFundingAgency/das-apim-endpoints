namespace SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;

public record GetCreateAccountTaskListQueryResponse
{
    public string HashedAccountId { get; set; }
    public bool HasPayeScheme { get; set; }
    public bool NameConfirmed { get; internal set; }
    public long? PendingAgreementId { get; internal set; }
    public bool AgreementAcknowledged { get; set; }
    public bool AddTrainingProviderAcknowledged { get; set; }
    public bool HasSignedAgreement { get; set; }
    public bool HasProviders { get; set; }
    public bool HasProviderPermissions { get; set; }
}