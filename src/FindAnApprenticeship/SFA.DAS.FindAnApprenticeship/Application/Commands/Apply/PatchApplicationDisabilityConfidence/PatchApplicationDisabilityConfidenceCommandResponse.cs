namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationDisabilityConfidence;

public record PatchApplicationDisabilityConfidenceCommandResponse
{
    public Domain.Models.Application Application { get; set; }
}