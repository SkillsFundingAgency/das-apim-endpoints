using System;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Domain.Reports;
public record Report
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public ReportType Type { get; set; }
    public ReportOwnerType OwnerType { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string DynamicCriteria { get; set; } = null!;
}