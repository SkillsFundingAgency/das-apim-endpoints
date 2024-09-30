using MediatR;

namespace SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Application.Commands;

public class UpsertApprenticeCommand : IRequest<UpsertApprenticeCommandResult>
{
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
}