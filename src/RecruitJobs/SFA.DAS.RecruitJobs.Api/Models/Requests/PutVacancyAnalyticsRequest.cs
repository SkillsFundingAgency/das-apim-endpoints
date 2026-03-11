using System.Collections.Generic;
using SFA.DAS.RecruitJobs.Domain.Models;

namespace SFA.DAS.RecruitJobs.Api.Models.Requests;

public record PutVacancyAnalyticsRequest
{
    public required List<VacancyAnalytics> AnalyticsData { get; set; } = [];
}