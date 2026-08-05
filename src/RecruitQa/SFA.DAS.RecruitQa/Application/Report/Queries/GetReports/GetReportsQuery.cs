using MediatR;

namespace SFA.DAS.RecruitQa.Application.Report.Queries.GetReports;

public record GetReportsQuery : IRequest<GetReportsQueryResult>;
