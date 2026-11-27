namespace SFA.DAS.RecruitQa.Domain.Reports;

public record Report
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public int DownloadCount { get; set; }
    public string DynamicCriteria { get; set; } = null!;
}
