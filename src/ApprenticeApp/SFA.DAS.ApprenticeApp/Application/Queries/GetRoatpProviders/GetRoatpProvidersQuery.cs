using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetRoatpProviders;

public record GetRoatpProvidersQuery : IRequest<GetRoatpProvidersQueryResult>;
