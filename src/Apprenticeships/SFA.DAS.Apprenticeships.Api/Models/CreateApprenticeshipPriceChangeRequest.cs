namespace SFA.DAS.Apprenticeships.Api.Models;

public class CreateApprenticeshipPriceChangeRequest
{
    public long? ProviderId { get; set; }
    public long? EmployerId { get; set; }
    public string UserId { get; set; } = null!;
    public decimal? TrainingPrice { get; set; }
    public decimal? AssessmentPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string Reason { get; set; } = null!;
    public DateTime EffectiveFromDate { get; set; }
}