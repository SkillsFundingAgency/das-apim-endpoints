using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetVacanciesByUkprn;
public record GetVacanciesByUkprnQueryResult
{
    public GetPagedVacancySummaryApiResponse.Info PageInfo { get; set; }
    public List<VacancySummary> VacancySummaries { get; set; }
    public ProviderTransferredVacanciesAlertModel ProviderTransferredVacanciesAlert { get; set; } = new();
    public WithdrawnVacanciesAlertModel WithdrawnVacanciesAlert { get; set; } = new();

    public static GetVacanciesByUkprnQueryResult FromResponses(
        GetPagedVacancySummaryApiResponse source,
        GetProviderAlertsApiResponse alertsSource)
    {
        return new GetVacanciesByUkprnQueryResult
        {
            PageInfo = source.PageInfo,
            VacancySummaries = source.Items,
            ProviderTransferredVacanciesAlert = alertsSource.ProviderTransferredVacanciesAlert ?? new ProviderTransferredVacanciesAlertModel(),
            WithdrawnVacanciesAlert = alertsSource.WithdrawnVacanciesAlert ?? new WithdrawnVacanciesAlertModel(),
        };
    }
}