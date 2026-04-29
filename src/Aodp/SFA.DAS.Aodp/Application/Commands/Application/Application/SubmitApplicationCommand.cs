using MediatR;
using SFA.DAS.Aodp.Validation;
namespace SFA.DAS.Aodp.Application.Commands.Application.Application;
public class SubmitApplicationCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    public Guid ApplicationId { get; set; }

    [AllowedCharacters(TextCharacterProfile.PersonName)]
    public string SubmittedBy { get; set; }

    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string SubmittedByEmail { get; set; }
}


