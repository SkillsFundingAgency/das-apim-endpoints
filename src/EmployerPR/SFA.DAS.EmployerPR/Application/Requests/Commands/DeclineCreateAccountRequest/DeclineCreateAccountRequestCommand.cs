using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineCreateAccountRequest;

public sealed class DeclineCreateAccountRequestCommand : IRequest<Unit>
{
    public required Guid RequestId { get; set; }
    public required string ActionedBy { get; set; }
}
