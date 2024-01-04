namespace SFA.DAS.Apprenticeships.Api.Models;

public class ApprenticeshipPriceResponse
{
    public Guid ApprenticeshipKey { get; set; }
    public int FundingBandMaximum { get; set; }
    public decimal TrainingPrice { get; set; }
    public decimal AssessmentPrice { get; set; }
    public DateTime EarliestEffectiveDate { get; set; }
    public DateTime? ApprenticeshipActualStartDate { get; set; }
    public DateTime? ApprenticeshipPlannedEndDate { get; set; }
    public DateTime? HardCloseDate { get; set; }
    public string? EmployerName { get; set; }
}