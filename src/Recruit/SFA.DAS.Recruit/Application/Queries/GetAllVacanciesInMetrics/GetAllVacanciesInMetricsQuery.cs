using MediatR;
using System;

namespace SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics
{
    public record GetAllVacanciesInMetricsQuery(
        DateTime StartDate,
        DateTime EndDate) : IRequest<GetAllVacanciesInMetricsQueryResult>;
}