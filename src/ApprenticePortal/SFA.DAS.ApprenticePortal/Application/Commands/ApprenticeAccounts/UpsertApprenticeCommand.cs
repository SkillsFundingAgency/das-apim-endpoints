using MediatR;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeAccounts;

public class UpsertApprenticeCommand : IRequest<UpsertApprenticeCommandResult>
{
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
}