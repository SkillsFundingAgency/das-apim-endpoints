using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectEmployer;

public class GetSelectEmployerQueryResult
{
    public List<AccountProviderLegalEntityItem> AccountProviderLegalEntities { get; init; } = [];
    public List<string> Employers { get; init; } = [];
}

public class AccountProviderLegalEntityItem
{
    public long AccountId { get; set; }
    public string AccountPublicHashedId { get; set; }
    public string AccountHashedId { get; set; }
    public string AccountName { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
    public string AccountLegalEntityName { get; set; }
    public long AccountProviderId { get; set; }
    public string ApprenticeshipEmployerType { get; set; }
}
