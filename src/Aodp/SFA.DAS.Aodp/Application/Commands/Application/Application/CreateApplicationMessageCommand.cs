using MediatR;
using SFA.DAS.Aodp.Validation;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class CreateApplicationMessageCommand : IRequest<BaseMediatrResponse<CreateApplicationMessageCommandResponse>>
{
    public Guid ApplicationId { get; set; }

    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string MessageText { get; set; }

    [MessageType]
    public string MessageType { get; set; }

    [UserType]
    public string UserType { get; set; }

    [AllowedCharacters(TextCharacterProfile.PersonName)]
    public string SentByName { get; set; }

    [AllowedCharacters(TextCharacterProfile.FreeText)]
    public string SentByEmail { get; set; }
}
