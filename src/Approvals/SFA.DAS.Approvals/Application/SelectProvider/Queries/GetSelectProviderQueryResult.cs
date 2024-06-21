using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

namespace SFA.DAS.Approvals.Application.SelectProvider.Queries;

public class GetSelectProviderQueryResult
{
    public IEnumerable<Provider> Providers { get; set; }

    public AccountLegalEntity AccountLegalEntity { get; set; } 
}

public class AccountLegalEntity
{
    public string LegalEntityName { get; set; }
}
