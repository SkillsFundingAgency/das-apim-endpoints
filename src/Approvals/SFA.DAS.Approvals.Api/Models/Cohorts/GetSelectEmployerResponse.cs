using System.Collections.Generic;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectEmployer;

namespace SFA.DAS.Approvals.Api.Models.Cohorts;

public class GetSelectEmployerResponse
{
    public List<AccountProviderLegalEntityResponseItem> AccountProviderLegalEntities { get; set; }
    public List<string> Employers { get; set; }
    public int TotalCount { get; set; }

    public static implicit operator GetSelectEmployerResponse(GetSelectEmployerQueryResult source)
    {
        return new GetSelectEmployerResponse
        {
            AccountProviderLegalEntities = source.AccountProviderLegalEntities?.ConvertAll(x => new AccountProviderLegalEntityResponseItem
            {
                AccountId = x.AccountId,
                AccountPublicHashedId = x.AccountPublicHashedId,
                AccountHashedId = x.AccountHashedId,
                AccountName = x.AccountName,
                AccountLegalEntityId = x.AccountLegalEntityId,
                AccountLegalEntityPublicHashedId = x.AccountLegalEntityPublicHashedId,
                AccountLegalEntityName = x.AccountLegalEntityName,
                AccountProviderId = x.AccountProviderId,
                ApprenticeshipEmployerType = x.ApprenticeshipEmployerType
            }) ?? [],
            Employers = source.Employers ?? [],
            TotalCount = source.TotalCount
        };
    }
}

public class AccountProviderLegalEntityResponseItem
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
