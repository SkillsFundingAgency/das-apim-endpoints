using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetAlertsByAccountId;
public record GetAlertsByAccountIdQueryResult
{
    public EmployerTransferredVacanciesAlertModel EmployerRevokedTransferredVacanciesAlert { get; set; } = new();
    public EmployerTransferredVacanciesAlertModel BlockedProviderTransferredVacanciesAlert { get; set; } = new();
    public BlockedProviderAlertModel BlockedProviderAlert { get; set; } = new();
    public WithdrawnVacanciesAlertModel WithDrawnByQaVacanciesAlert { get; set; } = new();

    public static GetAlertsByAccountIdQueryResult FromResponses(GetEmployerAlertsApiResponse alertsSource)
    {
        if (alertsSource is null) return new GetAlertsByAccountIdQueryResult();

        return new GetAlertsByAccountIdQueryResult
        {
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