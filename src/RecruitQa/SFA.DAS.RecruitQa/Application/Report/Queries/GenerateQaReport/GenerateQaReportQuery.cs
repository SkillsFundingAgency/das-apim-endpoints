using MediatR;

namespace SFA.DAS.RecruitQa.Application.Report.Queries.GenerateQaReport;

public record GenerateQaReportQuery(Guid ReportId) : IRequest<GenerateQaReportQueryResult>;
