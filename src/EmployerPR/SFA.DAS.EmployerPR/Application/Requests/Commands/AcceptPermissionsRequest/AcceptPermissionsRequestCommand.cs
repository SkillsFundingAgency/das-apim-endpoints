using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptPermissionsRequest;

public sealed class AcceptPermissionsRequestCommand : IRequest<Unit>
{
    public required Guid RequestId { get; set; }
    public required string ActionedBy { get; set; }
}
