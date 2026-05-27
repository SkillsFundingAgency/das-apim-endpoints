using System.Collections.Generic;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;

public sealed record GetProviderPermissionsByUkprnAndAccountIdQueryResult(List<GetAccountLegalEntityResponseItem> LegalEntities);