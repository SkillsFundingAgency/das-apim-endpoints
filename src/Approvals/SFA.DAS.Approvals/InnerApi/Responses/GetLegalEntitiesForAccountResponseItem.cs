using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetLegalEntitiesForAccountResponseItem
{
    public List<Agreement> Agreements { get; set; }
    public string Address { get; set; }
    public string Name { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
    public long LegalEntityId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string HashedAccountId { get; set; }
}