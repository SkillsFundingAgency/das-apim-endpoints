using MediatR;
using System;

namespace SFA.DAS.Recruit.Application.Report.Query.GetReportDataById;

public record GetReportDataByIdQuery(Guid ReportId) : IRequest<GetReportDataByIdQueryResult>;
