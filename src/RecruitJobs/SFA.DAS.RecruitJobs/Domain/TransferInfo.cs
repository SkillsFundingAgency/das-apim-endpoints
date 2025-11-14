namespace SFA.DAS.RecruitJobs.Domain;

public record TransferInfo
{
    public long Ukprn { get; set; }
    public string ProviderName { get; set; }
    public string LegalEntityName { get; set; }
    public DateTime TransferredDate { get; set; }
    public TransferReason Reason { get; set; }
}