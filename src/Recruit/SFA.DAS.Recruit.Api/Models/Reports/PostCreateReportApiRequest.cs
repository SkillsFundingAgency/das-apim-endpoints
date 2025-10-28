using SFA.DAS.Recruit.Enums;
using System;

namespace SFA.DAS.Recruit.Api.Models.Reports;

public record PostCreateReportApiRequest
{
    public string Name { get; init; } = null!;
    public Guid UserId { get; init; }
    public string CreatedBy { get; init; } = null!;
    public required DateTime FromDate { get; init; }
    public required DateTime ToDate { get; init; }
    public int? Ukprn { get; init; }
    public required ReportOwnerType OwnerType { get; init; }
}