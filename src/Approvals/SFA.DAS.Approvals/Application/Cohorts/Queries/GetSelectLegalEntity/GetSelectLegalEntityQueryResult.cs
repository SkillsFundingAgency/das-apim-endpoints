using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectLegalEntity;

public class GetSelectLegalEntityQueryResult
{
    public List<GetLegalEntitiesForAccountResponseItem> LegalEntities { get; set; }
}