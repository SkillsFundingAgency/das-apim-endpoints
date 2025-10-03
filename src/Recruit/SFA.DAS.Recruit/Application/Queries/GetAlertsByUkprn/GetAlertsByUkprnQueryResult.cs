using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetAlertsByUkprn;
public record GetAlertsByUkprnQueryResult
{
    public ProviderTransferredVacanciesAlertModel ProviderTransferredVacanciesAlert { get; set; } = new();
    public WithdrawnVacanciesAlertModel WithdrawnVacanciesAlert { get; set; } = new();

    public static GetAlertsByUkprnQueryResult FromResponses(GetProviderAlertsApiResponse alertsSource)
    {
        if (alertsSource is null) return new GetAlertsByUkprnQueryResult();

        return new GetAlertsByUkprnQueryResult
        {
            ProviderTransferredVacanciesAlert = alertsSource.ProviderTransferredVacanciesAlert ?? new ProviderTransferredVacanciesAlertModel(),
            WithdrawnVacanciesAlert = alertsSource.WithdrawnVacanciesAlert ?? new WithdrawnVacanciesAlertModel(),
        };
    }
}