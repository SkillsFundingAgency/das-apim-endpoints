using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;

public sealed record GetProviderPermissionsByUkprnAndAccountIdQuery(int Ukprn, long AccountId) : IRequest<GetProviderPermissionsByUkprnAndAccountIdQueryResult>;