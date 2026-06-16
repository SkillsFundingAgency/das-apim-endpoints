using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.Recruit.Application.Queries.GetEmployerAccountLegalEntities;

public record GetEmployerAccountLegalEntitiesQuery(string AccountHashedId, List<Operation> Operations)
    : MediatR.IRequest<GetEmployerAccountLegalEntitiesQueryResult>;
