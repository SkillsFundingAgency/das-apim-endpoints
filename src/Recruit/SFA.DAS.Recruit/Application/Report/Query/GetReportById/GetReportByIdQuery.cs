using MediatR;
using System;

namespace SFA.DAS.Recruit.Application.Report.Query.GetReportById;

public record GetReportByIdQuery(Guid ReportId) : IRequest<GetReportByIdQueryResult>;