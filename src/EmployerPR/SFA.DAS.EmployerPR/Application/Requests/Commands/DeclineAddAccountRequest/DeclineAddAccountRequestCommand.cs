using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineAddAccountRequest;

public sealed class DeclineAddAccountRequestCommand : IRequest<Unit>
{
    public required Guid RequestId { get; set; }
    public required string ActionedBy { get; set; }
}
