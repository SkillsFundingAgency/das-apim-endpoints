using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;

public sealed record GetProviderPermissionsByUkprnAndAccountIdQuery(int Ukprn, long AccountId, List<Operation> Operations)
    : IRequest<GetProviderPermissionsByUkprnAndAccountIdQueryResult>;