using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.Recruit.Api.Models;

public sealed record GetProviderPermissionsResponse(List<AccountLegalEntityItem> Permissions);