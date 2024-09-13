using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineRequest;

public sealed class DeclinedRequestCommand : IRequest<Unit>
{
    public required Guid RequestId { get; set; }
    public required string ActionedBy { get; set; }
}
