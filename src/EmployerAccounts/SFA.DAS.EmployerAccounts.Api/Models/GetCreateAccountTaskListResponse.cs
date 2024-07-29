using SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;

namespace SFA.DAS.EmployerAccounts.Api.Models;

public record GetCreateAccountTaskListResponse
{
    public string HashedAccountId { get; set; }
    public bool HasPayeScheme { get; set; }

    public int CompletedSections =>
        // by default, will have 1 completed section for user details (step previous)
        AgreementAcknowledged ? 4 : NameConfirmed ? 3 : HasPayeScheme ? 2 : 1;

    public bool NameConfirmed { get; internal set; }
    public long? PendingAgreementId { get; internal set; }
    public bool AgreementAcknowledged { get; set; }
    public bool AddTrainingProviderAcknowledged { get; set; }
    public bool HasSignedAgreement { get; set; }
    public bool HasProviders { get; set; }
    public bool HasProviderPermissions { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }

    public static implicit operator GetCreateAccountTaskListResponse(GetCreateAccountTaskListQueryResponse source)
    {
        return new GetCreateAccountTaskListResponse
        {
            HashedAccountId = source.HashedAccountId,
            HasProviders = source.HasProviders,
            AddTrainingProviderAcknowledged = source.AddTrainingProviderAcknowledged.GetValueOrDefault(),
            AgreementAcknowledged = source.AgreementAcknowledged,
            NameConfirmed = source.NameConfirmed.GetValueOrDefault(),
            HasPayeScheme = source.HasPayeScheme,
            HasProviderPermissions = source.HasProviderPermissions,
            HasSignedAgreement = source.HasSignedAgreement,
            PendingAgreementId = source.PendingAgreementId,
            UserFirstName = source.UserFirstName,
            UserLastName = source.UserLastName
        };
    }
}