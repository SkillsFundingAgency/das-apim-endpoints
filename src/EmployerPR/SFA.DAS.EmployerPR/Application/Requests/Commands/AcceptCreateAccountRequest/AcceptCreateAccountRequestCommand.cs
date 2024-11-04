using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptCreateAccountRequest;

public record AcceptCreateAccountRequestCommand(Guid RequestId, string FirstName, string LastName, string Email, Guid UserRef) : IRequest;
