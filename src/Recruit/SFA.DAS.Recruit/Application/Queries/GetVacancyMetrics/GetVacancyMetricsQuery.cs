using MediatR;
using System;

namespace SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics
{
    public record GetVacancyMetricsQuery(
        DateTime StartDate,
        DateTime EndDate) : IRequest<GetVacancyMetricsQueryResult>;
}
