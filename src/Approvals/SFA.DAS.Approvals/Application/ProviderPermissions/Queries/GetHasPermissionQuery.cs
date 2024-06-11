using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public record  GetHasPermissionQuery(long? Ukprn, long? AccountLegalEntityId, Operation Operation) : IRequest<bool>
{
}