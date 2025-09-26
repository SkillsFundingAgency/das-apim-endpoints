using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetVacanciesByAccountId;
public record GetVacanciesByAccountIdQueryResult
{
    public GetPagedVacancySummaryApiResponse.Info PageInfo { get; set; }
    public List<VacancySummary> VacancySummaries { get; set; }
    public EmployerTransferredVacanciesAlertModel EmployerRevokedTransferredVacanciesAlert { get; set; } = new();
    public EmployerTransferredVacanciesAlertModel BlockedProviderTransferredVacanciesAlert { get; set; } = new();
    public BlockedProviderAlertModel BlockedProviderAlert { get; set; } = new();
    public WithdrawnVacanciesAlertModel WithDrawnByQaVacanciesAlert { get; set; } = new();

    public static GetVacanciesByAccountIdQueryResult FromResponses(
        GetPagedVacancySummaryApiResponse source,
        GetEmployerAlertsApiResponse alertsSource)
    {
        return new GetVacanciesByAccountIdQueryResult
        {
            PageInfo = source.PageInfo,
            VacancySummaries = source.Items,
            EmployerRevokedTransferredVacanciesAlert = alertsSource.EmployerRevokedTransferredVacanciesAlert
                                                       ?? new EmployerTransferredVacanciesAlertModel(),
            BlockedProviderTransferredVacanciesAlert = alertsSource.BlockedProviderTransferredVacanciesAlert
                                                       ?? new EmployerTransferredVacanciesAlertModel(),
            BlockedProviderAlert = alertsSource.BlockedProviderAlert
                                   ?? new BlockedProviderAlertModel(),
            WithDrawnByQaVacanciesAlert = alertsSource.WithDrawnByQaVacanciesAlert
                                          ?? new WithdrawnVacanciesAlertModel()
        };
    }
}