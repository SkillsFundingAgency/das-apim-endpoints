using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;

public sealed class DeclinePermissionsRequestCommand : IRequest<Unit>
{
    public required Guid RequestId { get; set; }
    public required string ActionedBy { get; set; }
}
