using SFA.DAS.SharedOuterApi.Types.Models;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetEmployerAccountLegalEntities;

public sealed record GetEmployerAccountLegalEntitiesQueryResult(List<LegalEntityItem> LegalEntities);
