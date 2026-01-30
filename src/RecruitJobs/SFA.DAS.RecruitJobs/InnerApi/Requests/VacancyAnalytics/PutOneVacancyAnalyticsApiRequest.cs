using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using static SFA.DAS.RecruitJobs.InnerApi.Requests.VacancyAnalytics.PutOneVacancyAnalyticsApiRequest;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.VacancyAnalytics;

public record PutOneVacancyAnalyticsApiRequest(long VacancyReference, VacancyAnalyticsRequestData Payload) : IPutApiRequest
{
    public string PutUrl => $"api/vacancyAnalytics/{VacancyReference}";
    public object Data { get; set; } = Payload;

    public record VacancyAnalyticsRequestData
    {
        public required List<Core.Domain.Models.VacancyAnalytics> AnalyticsData { get; set; }
    }
}