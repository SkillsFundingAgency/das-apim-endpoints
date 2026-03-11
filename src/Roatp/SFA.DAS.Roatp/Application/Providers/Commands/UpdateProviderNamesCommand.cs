using MediatR;

namespace SFA.DAS.Roatp.Application.Providers.Commands;

public record UpdateProviderNamesCommand() : IRequest<Unit>;
