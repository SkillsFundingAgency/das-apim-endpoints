using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;

public record GetRoatpProvidersQuery : IRequest<GetRoatpProvidersQueryResult>;
