using MediatR;

namespace SFA.DAS.Recruit.Application.Report.Query.GetReportsByUkprn;

public record GetReportsByUkprnQuery(int Ukprn) : IRequest<GetReportsByUkprnQueryResult>;