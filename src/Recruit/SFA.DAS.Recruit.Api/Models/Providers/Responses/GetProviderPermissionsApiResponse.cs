using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.Recruit.Api.Models.Providers.Responses;

public sealed record GetProviderPermissionsApiResponse
{
    public List<AccountLegalEntityItem> LegalEntities { get; init; } = [];
}