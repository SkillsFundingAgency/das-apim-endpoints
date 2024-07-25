using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;
public record GetAccountProviderLegalEntitiesQuery(int? Ukprn, Operation[] Operations, string AccountHashedId) : IRequest<GetProviderAccountLegalEntitiesResponse>
{
}
