using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprn;

public sealed record GetProviderPermissionsByUkprnQuery(int Ukprn, List<Operation> Operations) : IRequest<GetProviderPermissionsByUkprnQueryResult>;