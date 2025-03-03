namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationStatus;

public record PatchApplicationStatusCommandResponse
{
    public Domain.Models.Application Application { get; set; }
}