using SFA.DAS.SharedOuterApi.Types.Models;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissions;

public sealed record GetProviderPermissionsByUkprnQueryResult(List<AccountLegalEntityItem> Permissions);
