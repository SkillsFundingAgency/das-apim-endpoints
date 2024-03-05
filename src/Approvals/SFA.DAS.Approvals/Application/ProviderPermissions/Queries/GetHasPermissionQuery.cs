using MediatR;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public record  GetHasPermissionQuery(long? Ukprn, long? AccountLegalEntityId, string Operation) : IRequest<bool>
{
}