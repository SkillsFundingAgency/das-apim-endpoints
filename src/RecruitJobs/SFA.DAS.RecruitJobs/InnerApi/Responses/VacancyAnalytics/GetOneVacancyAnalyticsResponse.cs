using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.InnerApi.Responses.VacancyAnalytics;

public record GetOneVacancyAnalyticsResponse
{
    public required long VacancyReference { get; init; }
    public DateTime UpdatedDate { get; init; }
    public List<Core.Domain.Models.VacancyAnalytics> Analytics { get; set; } = [];
}