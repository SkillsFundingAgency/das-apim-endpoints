using SFA.DAS.Recruit.Enums;
using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.Report.Command.PostCreateReport;

public record PostCreateReportCommand : IRequest
{
    public required Guid Id { get; set; }
    public string Name { get; init; } = null!;
    public Guid UserId { get; init; }
    public string CreatedBy { get; init; } = null!;
    public required DateTime FromDate { get; init; }
    public required DateTime ToDate { get; init; }
    public int? Ukprn { get; init; }
    public required ReportOwnerType OwnerType { get; init; }
}