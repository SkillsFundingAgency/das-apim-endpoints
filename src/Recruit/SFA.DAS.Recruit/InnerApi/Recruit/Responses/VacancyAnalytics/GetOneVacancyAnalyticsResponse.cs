using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses.VacancyAnalytics;

public record GetOneVacancyAnalyticsResponse
{
    public required long VacancyReference { get; init; }
    public DateTime UpdatedDate { get; init; }
    public List<Domain.VacancyAnalytics.VacancyAnalytics> Analytics { get; set; } = [];
}