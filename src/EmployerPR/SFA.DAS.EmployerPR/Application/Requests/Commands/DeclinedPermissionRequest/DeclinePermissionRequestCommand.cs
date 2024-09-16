using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinedRequest;

public sealed class DeclinePermissionRequestCommand : IRequest<Unit>
{
    public required Guid RequestId { get; set; }
    public required string ActionedBy { get; set; }
}
