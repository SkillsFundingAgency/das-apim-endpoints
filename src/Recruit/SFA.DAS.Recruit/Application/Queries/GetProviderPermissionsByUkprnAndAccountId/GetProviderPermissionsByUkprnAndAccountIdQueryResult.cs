using SFA.DAS.SharedOuterApi.Types.Models;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;

public sealed record GetProviderPermissionsByUkprnAndAccountIdQueryResult(List<LegalEntityItem> LegalEntities);