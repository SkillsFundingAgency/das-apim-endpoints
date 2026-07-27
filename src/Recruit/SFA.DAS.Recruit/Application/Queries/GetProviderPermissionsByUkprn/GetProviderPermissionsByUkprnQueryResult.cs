using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprn;

public sealed record GetProviderPermissionsByUkprnQueryResult(List<LegalEntityItem> LegalEntities);