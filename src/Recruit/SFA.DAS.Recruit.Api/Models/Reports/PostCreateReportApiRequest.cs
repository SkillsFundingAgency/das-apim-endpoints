using SFA.DAS.Recruit.Enums;
using System;

namespace SFA.DAS.Recruit.Api.Models.Reports;

public record PostCreateReportApiRequest
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string UserId { get; init; }
    public required string CreatedBy { get; init; }
    public required DateTime FromDate { get; init; }
    public required DateTime ToDate { get; init; }
    public int? Ukprn { get; init; }
    public required ReportOwnerType OwnerType { get; init; }
}