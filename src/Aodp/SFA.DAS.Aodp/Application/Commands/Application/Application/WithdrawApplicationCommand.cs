using MediatR;
using SFA.DAS.Aodp.Validation;
namespace SFA.DAS.Aodp.Application.Commands.Application.Application;
public class WithdrawApplicationCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    public Guid ApplicationId { get; set; }

    [AllowedCharacters(TextCharacterProfile.PersonName)]
    public required string WithdrawnBy { get; set; }

    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public required string WithdrawnByEmail { get; set; }
}