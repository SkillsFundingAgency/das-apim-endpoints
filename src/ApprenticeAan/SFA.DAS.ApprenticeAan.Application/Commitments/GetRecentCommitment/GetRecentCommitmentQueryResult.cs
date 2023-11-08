namespace SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;

public class GetRecentCommitmentQueryResult
{
    public long ApprenticeshipId { get; set; }
    public string Uln { get; set; } = null!;
    public string TrainingCode { get; set; } = null!;
    public string StandardUId { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? StopDate { get; set; }
    public int PaymentStatus { get; set; }
    public long Ukprn { get; set; }
    public string EmployerName { get; set; } = null!;
}

