using MediatR;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public record GetHasRelationshipWithPermissionQuery(long? Ukprn, Operation Operation) : IRequest<bool>
{
}