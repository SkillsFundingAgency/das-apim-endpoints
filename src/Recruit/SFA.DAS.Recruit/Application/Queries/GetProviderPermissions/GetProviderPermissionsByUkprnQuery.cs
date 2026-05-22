using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissions;

public sealed record GetProviderPermissionsByUkprnQuery(int Ukprn) : IRequest<GetProviderPermissionsByUkprnQueryResult>;