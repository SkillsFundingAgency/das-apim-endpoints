using MediatR;
using System;

namespace SFA.DAS.Recruit.Application.Report.Query.GenerateReportsById;

public record GenerateReportsByReportIdQuery(Guid ReportId) : IRequest<GenerateReportsByReportIdQueryResult>;