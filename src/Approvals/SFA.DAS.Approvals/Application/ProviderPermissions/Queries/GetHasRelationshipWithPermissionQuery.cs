using MediatR;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public record GetHasRelationshipWithPermissionQuery(long Ukprn) : IRequest<bool>;
