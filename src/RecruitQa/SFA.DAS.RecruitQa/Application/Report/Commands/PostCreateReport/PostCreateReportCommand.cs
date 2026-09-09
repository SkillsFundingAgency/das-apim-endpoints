using MediatR;

namespace SFA.DAS.RecruitQa.Application.Report.Commands.PostCreateReport;

public record PostCreateReportCommand : IRequest
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string UserId { get; init; }
    public required string CreatedBy { get; init; }
    public required DateTime FromDate { get; init; }
    public required DateTime ToDate { get; init; }
}
