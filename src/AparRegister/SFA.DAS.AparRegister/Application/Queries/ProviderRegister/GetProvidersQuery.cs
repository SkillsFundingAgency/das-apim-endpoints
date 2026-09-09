using MediatR;

namespace SFA.DAS.AparRegister.Application.Queries.ProviderRegister;

public record GetProvidersQuery : IRequest<GetProvidersQueryResult>;
