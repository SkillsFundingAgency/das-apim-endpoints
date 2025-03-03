namespace SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;

public record GetCreateAccountTaskListQueryResponse
{
    public string HashedAccountId { get; set; }
    public bool HasPayeScheme { get; set; }
    public bool? NameConfirmed { get;  set; }
    public long? PendingAgreementId { get; set; }
    public bool AgreementAcknowledged { get; set; }
    public bool? AddTrainingProviderAcknowledged { get; set; }
    public bool HasSignedAgreement { get; set; }
    public bool HasProviders { get; set; }
    public bool HasProviderPermissions { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public int CompletedSections =>
        // by default, will have 1 completed section for user details (step previous)
        AgreementAcknowledged ? 4 : NameConfirmed.GetValueOrDefault() ? 3 : HasPayeScheme ? 2 : 1;
}