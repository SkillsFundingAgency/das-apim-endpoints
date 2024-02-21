namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Models;

public record Education
{
    public string Institution { get; set; }

    public int FromYear { get; set; }

    public int ToYear { get; set; }
}