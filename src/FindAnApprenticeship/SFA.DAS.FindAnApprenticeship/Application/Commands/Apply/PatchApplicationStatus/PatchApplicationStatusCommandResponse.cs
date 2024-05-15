namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationStatus;

public record PatchApplicationStatusCommandResponse
{
    public Models.Application Application { get; set; }
}