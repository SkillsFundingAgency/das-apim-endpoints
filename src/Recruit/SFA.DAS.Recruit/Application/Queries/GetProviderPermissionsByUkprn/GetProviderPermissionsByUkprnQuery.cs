using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprn;

public sealed record GetProviderPermissionsByUkprnQuery(int Ukprn) : IRequest<GetProviderPermissionsByUkprnQueryResult>;